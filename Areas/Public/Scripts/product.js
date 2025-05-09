import { Log, QuerySelector, Get, Post } from './modules/utils.js';

let
  _root = '#product-details-form',
  _saveRoute,
  _productId,
  _registrationId;

const
  events = {
    onRegistration_Change: ({ target }) => {
      const
        { list, value } = target,
        val = list.querySelector(`:root [value="${value}"]`);

      _registrationId = val.dataset.registrationId;
    },
    onAdd_Click: async () => {
      const
        success = await Post(_saveRoute, {
          productId: _productId,
          giftRegistryId: _registrationId
        });

      Log(`Success: ${success}`);
    }
  }

const
  init = (saveRoute, propductId) => {
    _saveRoute = saveRoute;
    _productId = propductId
    setTemplate();
    setEvents();
  },
  setEvents = () => {
    const
      btn = QuerySelector('[data-registry-button-add]', _root),
      input = QuerySelector('[data-registration-input-id]', _root);

    btn.addEventListener('click', events.onAdd_Click);
    input.addEventListener('change', events.onRegistration_Change);
  },
  setTemplate = () => {
    const
      link = QuerySelector('[data-registry-link-add]', _root),
      clone = link.content.cloneNode(true),
      overview = document.querySelector('.overview .overview-buttons');

    overview.prepend(clone);
  };

export { init }