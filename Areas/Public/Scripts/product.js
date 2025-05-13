import { Log, QuerySelector, Post } from './modules/utils.js';

let
  _root = '#product-details-form',
  _saveRoute,
  _productId,
  _registrationId;

const
  events = {
    onAdd_Click: async ({ target }) => {
      const { dataset } = target;
      _registrationId = dataset.registryId;
      addToRegistry();
    }
  }

const
  init = (saveRoute, propductId, regCount) => {
    _saveRoute = saveRoute;
    _productId = propductId
    setTemplate();
    setEvents();
    Log(regCount);
  },
  setEvents = () => {
    const
      btn = QuerySelector('[data-registry-button-add]', _root);

    btn.addEventListener('click', events.onAdd_Click);
  },
  setTemplate = () => {
    const
      link = QuerySelector('[data-registry-link-add]', _root),
      clone = link.content.cloneNode(true),
      overview = document.querySelector('.overview .overview-buttons');

    overview.prepend(clone);
  },
  addToRegistry = async () => {
    const
      success = await Post(_saveRoute, {
        productId: _productId,
        giftRegistryId: _registrationId
      });

    Log(`Success: ${success}`);
  };

export { init }