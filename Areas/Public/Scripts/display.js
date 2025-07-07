import {
  QuerySelector, QuerySelectorAll,
  Post, Delete, DisplayNotification,
  UseTemplateTag, FadeOut
} from '../../modules/utils.js';

let
  _addToCartRoute,
  _removeRoute;

const
  init = (removeRoute, addToCartRoute) => {
    _addToCartRoute = addToCartRoute;
    _removeRoute = removeRoute;
    setFormEvents();
  },
  setFormEvents = () => {
    const
      forms = QuerySelectorAll('form[data-fulfilled="False"]', '[data-registry-product-list]');

    forms.forEach(form => {
      const
        { dataset } = form,
        [add, remove] = form.querySelectorAll('button');

      add.addEventListener('click', () => events.onAddToCart_Click(dataset));
      remove?.addEventListener('click', () => events.onRemoveItem_Click(dataset));
    });
  },
  promptUserQuantity = async (requestedQuantity, productName, owner) => {

    if (+requestedQuantity <= 1) {
      return Promise.resolve({ cancel: false, pledge: 1, onRemove: () => { } });
    }

    const { component: dialog, onRemove } = UseTemplateTag(
      '[data-template-dialog-quantity]',
      '[data-registry-product-list]',
      '[data-dialog-quantity]'
    );

    const
      input = dialog.querySelector(':scope input'),
      label = dialog.querySelector(':scope label'),
      cancel = dialog.querySelector(':scope [data-cancel]'),
      submit = dialog.querySelector(':scope [data-submit'),
      onFadeComplete = await FadeOut(dialog);;

    label.innerHTML = `<strong>${owner}</strong> has requested <strong>${requestedQuantity}</strong> of <strong>${productName}</strong>, Would you like to purchase more than one?`;
    input.max = requestedQuantity;

    dialog.showModal();

    onFadeComplete();

    return new Promise((res) => {
      submit.addEventListener('click', () => res({ pledge: input.value, cancel: false, onRemove }));
      cancel.addEventListener('click', () => res({ pledge: input.value, cancel: true, onRemove }));
    });
  }

const events = {
  onAddToCart_Click: async ({ registryItemId, quantity, productName, owner }) => {
    const
      ui = document.querySelector('.cart-qty'),
      { cancel, pledge, onRemove } = await promptUserQuantity(quantity, productName, owner);

    onRemove();

    if (cancel) {
      return;
    }

    const
      { success, data } = await Post(`${_addToCartRoute}?registryItemId=${registryItemId}&quantity=${pledge}`);

    if (!success) {
      DisplayNotification("Unable to Add Item to Bag", true);
    }

    if (Array.isArray(data) && data.some(x => x)) {
      DisplayNotification(data.join(','), true);
      return;
    }

    ui.innerHTML = '(1)'; //TODO: will need to return actual cart count
    DisplayNotification("Item Added to Bag", true);
  },
  onRemoveItem_Click: async ({ registryItemId }) => {
    const
      success = await Delete(`${_removeRoute}?id=${registryItemId}`);

    if (!success) {
      return;
    }

    const
      registryItem = QuerySelector(`[data-registry-item-id="${registryItemId}"]`, '[data-registry-product-list]');

    await FadeOut(registryItem);

    registryItem.remove();
  }
}

export { init }