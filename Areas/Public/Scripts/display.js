import { QuerySelectorAll, Post, Delete } from './modules/utils.js';
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
      forms = QuerySelectorAll('form', '[data-registry-product-list]');

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
  }
}

export { init }