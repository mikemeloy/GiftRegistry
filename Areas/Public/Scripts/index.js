import {
  Get, Post, Delete,
  Loading, LogError, GetInputValue,
  QuerySelector, SetInputValue,
  DateToInputString, DisplayNotification,
  AddQueryParamToURL, GetQueryParam,
  ToCurrency, FadeIn, FadeOut
} from '../../modules/utils.js';

let
  _currentUser,
  _searchRoute,
  _insertRoute,
  _updateRoute,
  _deleteRoute,
  _deleteItemRoute,
  _getRoute,
  _debounce;

const
  init = (searchUrl, insertRoute, deleteRoute, getRoute, updateRoute, currentUser, deleteItemRoute) => {
    _searchRoute = searchUrl;
    _insertRoute = insertRoute;
    _deleteRoute = deleteRoute;
    _getRoute = getRoute;
    _updateRoute = updateRoute;
    _currentUser = currentUser;
    _deleteItemRoute = deleteItemRoute;

    setFormEvents();
    setSearchByUrl();
  },
  setSearchByUrl = () => {
    const
      searchParam = GetQueryParam('search'),
      searchInput = querySelector('form input');

    if (!searchParam || !searchInput) {
      return;
    }

    searchInput.value = searchParam;
    events.onSearch_KeyUp();

  },
  setFormEvents = () => {
    const
      form = querySelector('[data-search]'),
      btnAdd = querySelector('[data-add]'),
      btnMine = querySelector('[data-search-mine]'),
      btnSave = querySelector('[data-registry-save]'),
      btnClose = querySelector('[data-modal-close]');

    form.addEventListener('keyup', events.onSearch_KeyUp);

    btnMine.addEventListener('click', events.onShowUser_Click);
    btnAdd.addEventListener('click', events.onAdd_Click);
    btnSave.addEventListener('click', events.onSave_Click);
    btnClose.addEventListener('click', events.onClose_Click)
  },
  debounce = (el) => {
    clearTimeout(_debounce);
    _debounce = setTimeout(async () => await el?.(), 400);
  },
  querySelector = (selector) => QuerySelector(selector, '[data-registry]'),
  getInputValue = (selector, options) => GetInputValue(selector, '[data-registry]', options),
  prepareModal = (name = '', desc = '', date = '', note = '', id = '', eventType = '0', title = 'Create a New Registry', sponsor = '', shipping = '0') => {
    const
      set = (selector, val) => SetInputValue(`[${selector}]`, '[data-modal-add]', val),
      header = querySelector('[data-modal-add] h4');

    set('data-add-id', id);
    set('data-add-name', name);
    set('data-add-description', desc);
    set('data-add-event-date', DateToInputString(date));
    set('data-add-notes', note);
    set('data-add-event-type', eventType);
    set('data-add-sponsor', sponsor);
    set('data-add-event-shipping-method', shipping);
    header.innerHTML = title;

    return querySelector('[data-modal-add]');
  },
  generateItemRow = (registryItems) => {
    const
      headerTemplate = querySelector('[data-template-registry-item-header]'),
      headerClone = headerTemplate.content.cloneNode(true),
      rowTemplate = querySelector('[data-template-registry-item-row]'),
      container = querySelector('[data-registry-item]');

    container.replaceChildren(headerClone)

    for (const registryItem of registryItems) {
      const
        { Id, Name, Price, Quantity, Purchased } = registryItem,
        clone = rowTemplate.content.cloneNode(true),
        btnDelete = clone.querySelector('[data-action-delete]'),
        btnEdit = clone.querySelector('[data-action-edit]'),
        actionRow = clone.querySelector('[data-actions]'),
        setValue = (selector, value) => {
          const el = clone.querySelector(`[data-${selector}]`);

          if (!el) {
            return;
          }

          el.dataset.deleteId = Id;
          el.innerHTML = value;
        };

      setValue('name', Name);
      setValue('price', ToCurrency(Price));
      setValue('quantity', Quantity);
      setValue('purchased', Purchased);
      actionRow.dataset.deleteId = Id;

      btnEdit.addEventListener('click', () =>
        events.onRegistryitemEdit_Click(registryItem)
      );

      btnDelete.addEventListener('click', () =>
        events.onRegistryitemDelete_Click(Id)
      );

      container.appendChild(clone);
    }
  };

const
  events = {
    onSearch_KeyUp: async () => {
      debounce(async () => {
        try {
          const
            query = getInputValue('[data-search] input'),
            { success, data, error } = await Get(_searchRoute, { query }),
            rsltWindow = querySelector('[data-result]');

          if (!success) {
            LogError('Failed to Search', error);
            return;
          }

          AddQueryParamToURL([{ key: 'search', value: query }]);

          const
            onComplete = await FadeOut(rsltWindow);

          rsltWindow.replaceChildren();
          rsltWindow.insertAdjacentHTML('afterbegin', data);

          const
            deleteBtns = rsltWindow.querySelectorAll('[data-registry-delete]'),
            editBtns = rsltWindow.querySelectorAll('[data-registry-edit]');

          deleteBtns.forEach(btn => btn.addEventListener('click', events.onDelete_Click));
          editBtns.forEach(btn => btn.addEventListener('click', events.onEdit_Click));

          await onComplete();

        } catch (error) {
          LogError('Failed to Search', error);
        }
      });
    },
    onAdd_Click: () => {
      const
        modal = prepareModal();

      modal.showModal();
    },
    onSave_Click: async () => {
      const
        endLoading = Loading(),
        id = getInputValue('[data-add-id]', { isNumeric: true }),
        name = getInputValue('[data-add-name]'),
        description = getInputValue('[data-add-description]'),
        notes = getInputValue('[data-add-notes]'),
        eventDate = getInputValue('[data-add-event-date]'),
        eventType = getInputValue('[data-add-event-type]', { isNumeric: true }),
        shippingOption = getInputValue('[data-add-event-shipping-method]', { isNumeric: true }),
        sponsor = getInputValue('[data-add-sponsor]'),
        url = (+id > 0) ? _updateRoute : _insertRoute;

      try {
        const
          { data: success } = await Post(url, {
            id,
            name,
            description,
            eventDate,
            eventType,
            notes,
            shippingOption,
            sponsor
          });


        if (success) {
          const
            dialog = querySelector('[data-modal-add]');

          dialog.close();
          DisplayNotification("Registry Saved...");
        }

      } catch (error) {
        LogError(error);
      } finally {
        endLoading();
      }
    },
    onDelete_Click: async ({ target }) => {
      const
        { dataset: { registryId } } = target;

      try {
        const
          success = await Delete(`${_deleteRoute}?id=${registryId}`);

        if (success) {
          events.onSearch_KeyUp();
        }
      } catch (error) {
        LogError(error);
        DisplayNotification("Unable to Delete Item");
      }
    },
    onEdit_Click: async ({ target }) => {
      const
        { dataset: { registryId } } = target;

      const
        { success, data } = await Get(`${_getRoute}?id=${registryId}`);

      if (!success) {
        alert('An Error has Occurred');
      }

      const
        {
          Id, Name, EventDate, EventType,
          Description, Notes, Sponsor,
          ShippingOption, RegistryItems
        } = data,
        modal = prepareModal(Name, Description, EventDate, Notes, Id, EventType, `Edit: ${Name}`, Sponsor, ShippingOption);

      modal.showModal();
      generateItemRow(RegistryItems);
    },
    onClose_Click: ({ target }) => {
      const
        dialog = target.closest('dialog');

      if (!dialog) {
        return;
      }

      dialog.close();
    },
    onShowUser_Click: () => {
      AddQueryParamToURL([{ key: 'search', value: _currentUser }]);
      setSearchByUrl();
    },
    onRegistryitemDelete_Click: async (registryItemId) => {
      const
        { success, data: deleted } = await Delete(`${_deleteItemRoute}?id=${registryItemId}`);

      if (!success || !deleted) {
        DisplayNotification(`Unable to Delete Item From Registry`);
        return;
      }

      const
        rows = document.querySelectorAll(`[data-delete-id="${registryItemId}"]`),
        actions = document.querySelector(`[data-actions][data-delete-id="${registryItemId}"]`);

      for (const el of rows) {
        el
          .animate({ opacity: [1, .2] }, { duration: 300, fill: "forwards" });
        console.dir(el);
      }

      actions?.remove();

    },
    onRegistryitemEdit_Click: (registryItem) => {
      console.dir(registryItem);
    }
  };

export { init }