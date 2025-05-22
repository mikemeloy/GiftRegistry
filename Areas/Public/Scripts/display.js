import { QuerySelector, QuerySelectorAll, Post, Delete } from './modules/utils.js';
let
  _addToCartRoute,
  _removeRoute;

const
  init = (removeRoute, addToCartRoute, addToCartLink) => {
    _addToCartRoute = addToCartRoute;
    _removeRoute = removeRoute;
    setFormEvents();
    console.dir(addToCartLink);
  },
  setFormEvents = () => {
    const
      forms = QuerySelectorAll('form[data-purchased="False"]', '[data-registry-product-list]');

    forms.forEach(form => {
      const
        { dataset } = form,
        [add, remove] = form.querySelectorAll('button');

      add.addEventListener('click', () => events.onAddToCart_Click(dataset));
      remove?.addEventListener('click', () => events.onRemoveItem_Click(dataset));
    });
  }

const events = {
  onAddToCart_Click: async ({ registryItemId }) => {
    const
      success = await Post(_addToCartRoute, registryItemId);
  },
  onRemoveItem_Click: async ({ registryItemId }) => {
    const
      success = await Delete(`${_removeRoute}?id=${registryItemId}`);

    if (!success) {
      return;
    }

    const
      duration = 500,
      registryItem = QuerySelector(`[data-registry-item-id="${registryItemId}"]`, '[data-registry-product-list]');

    registryItem.animate([{ opacity: 0 }], { duration });
    setTimeout(() => registryItem.remove(), duration - 1);
  }
}

export { init }