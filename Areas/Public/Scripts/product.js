import { QuerySelector, Post, DisplayNotification } from './modules/utils.js';

let
  _root = '#product-details-form',
  _saveRoute,
  _productId,
  _registryId,
  _quantity;

const
  events = {
    onAdd_Click: async ({ target }) => {
      const
        dialog = QuerySelector('[data-registry-select]', _root),
        { dataset } = target;
      _registryId = dataset.registryId;

      dialog.showModal();
    },
    onSubmit_Click: () => {
      const
        registryData = {
          productId: _productId,
          giftRegistryId: _registryId,
          quantity: _quantity
        };

      addToRegistry(registryData);
    },
    onCancel_Click: () => {
      const
        dialog = QuerySelector('[data-registry-select]', _root);

      dialog.close();
    },
    onRadio_Change: ({ target }) => {
      _registryId = target.value;
    },
    onQuantity_Change: ({ target }) => {
      _quantity = target.value;
    }
  }

const
  init = (saveRoute, propductId, registryId) => {
    _registryId = registryId;
    _saveRoute = saveRoute;
    _productId = propductId;

    setTemplate();
    setEvents();
  },
  setEvents = () => {
    const
      btnAdd = QuerySelector('[data-registry-button-add]', _root),
      btnCancel = QuerySelector('[data-cancel]', _root),
      btnSubmit = QuerySelector('[data-submit]', _root),
      txtQuantity = QuerySelector('[data-quantity]', _root),
      radios = document.querySelectorAll('input[name="registry-radio"]');//fix later

    btnAdd.addEventListener('click', events.onAdd_Click);
    btnCancel.addEventListener('click', events.onCancel_Click);
    btnSubmit.addEventListener('click', events.onSubmit_Click);
    txtQuantity.addEventListener('change', events.onQuantity_Change)
    radios.forEach(radio => radio.addEventListener('change', events.onRadio_Change))
  },
  setTemplate = () => {
    const
      link = QuerySelector('[data-registry-link-add]', _root),
      clone = link.content.cloneNode(true),
      overview = document.querySelector('.overview .overview-buttons');

    overview.prepend(clone);
  },
  addToRegistry = async (data) => {
    const
      dialog = QuerySelector('[data-registry-select]', _root),
      { data: success } = await Post(_saveRoute, data);

    if (!success) {
      DisplayNotification("Select a Registry and Quantity to Continue");
      return;
    }

    dialog.close();
    DisplayNotification("Item add to Registry");
  };

export { init }