import {
  QuerySelector,
  Post,
  DisplayNotification,
  LogError,
  NopDateControlValue,
  AttributeControlType
} from '../../modules/index.js';


let
  _root = '#product-details-form',
  _saveRoute,
  _productId,
  _registryId,
  _quantity = 1,
  _productAttributes = [];

const
  init = (saveRoute, propductId, registryId, attributes) => {
    _registryId = registryId;
    _saveRoute = saveRoute;
    _productId = propductId;
    _productAttributes = JSON.parse(attributes);

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
  },
  getAttributeControl = ({ AttributeId, ControlTypeId }) => {
    switch (ControlTypeId) {
      case AttributeControlType.Radio:
      case AttributeControlType.Checkbox:
        return document.querySelector(`input[id^=product_attribute_${AttributeId}]:checked`);
      case AttributeControlType.Date:
        return NopDateControlValue(`product_attribute_${AttributeId}`)
      default:
        return document.querySelector(`#product_attribute_${AttributeId}`);
    }
  },
  getProductAttributeFieldValues = () => {
    for (const attr of _productAttributes) {
      const input = getAttributeControl(attr);

      if (!input) {
        LogError(attr);
        continue;
      }

      attr.AttributeValue = input.value;
    }
  },
  requiredAttributesHaveValues = () =>
    _productAttributes.every(({ AttributeValue, IsRequired }) => {
      if (!IsRequired) {
        return true;
      }

      if (+AttributeValue === 0) {
        return false;
      }

      return Boolean(AttributeValue);
    });

const
  events = {
    onAdd_Click: async ({ target }) => {
      const
        dialog = QuerySelector('[data-registry-select]', _root),
        { dataset } = target;

      _registryId = dataset.registryId;

      getProductAttributeFieldValues();

      if (!requiredAttributesHaveValues()) {
        DisplayNotification("Please Fill Out All Requied Fields");
        return;
      }

      dialog.showModal();
    },
    onSubmit_Click: () => {
      const
        registryData = {
          productId: _productId,
          giftRegistryId: _registryId,
          quantity: _quantity,
          attributes: _productAttributes
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

export { init }