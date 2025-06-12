import {
  Get, Post, Delete,
  Loading, LogError, GetInputValue,
  QuerySelector, SetInputValue,
  DateToInputString, DisplayNotification,
  AddQueryParamToURL, GetQueryParam
} from './modules/utils.js';

let
  _searchRoute,
  _insertRoute,
  _updateRoute,
  _deleteRoute,
  _getRoute,
  _debounce;

const
  init = (searchUrl, insertRoute, deleteRoute, getRoute, updateRoute) => {
    _searchRoute = searchUrl;
    _insertRoute = insertRoute;
    _deleteRoute = deleteRoute;
    _getRoute = getRoute;
    _updateRoute = updateRoute

    setFormEvents();
    setSearchByUrl();
  },
  setSearchByUrl = () => {
    const
      searchParam = GetQueryParam('search'),
      input = querySelector('form input');

    if (!searchParam || !input) {
      return;
    }

    input.value = searchParam;
    events.onSearch_KeyUp();
  },
  setFormEvents = () => {
    const
      form = querySelector('[data-search]'),
      btnAdd = querySelector('[data-add]'),
      btnSave = querySelector('[data-registry-save]');

    form.addEventListener('keyup', events.onSearch_KeyUp);
    btnAdd.addEventListener('click', events.onAdd_Click);
    btnSave.addEventListener('click', events.onSave_Click);
  },
  debounce = (el) => {
    clearTimeout(_debounce);
    _debounce = setTimeout(async () => await el?.(), 400);
  },
  querySelector = (selector) => QuerySelector(selector, '[data-registry]'),
  getInputValue = (selector) => GetInputValue(selector, '[data-registry]'),
  prepareModal = (name = '', desc = '', date = '', note = '', id = '') => {
    const
      set = (selector, val) => SetInputValue(`[${selector}]`, '[data-modal-add]', val);

    set('data-add-id', id);
    set('data-add-name', name);
    set('data-add-description', desc);
    set('data-add-event-date', DateToInputString(date));
    set('data-add-notes', note);

    return querySelector('[data-modal-add]');
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

          rsltWindow
            .animate({ opacity: [1, 0] }, { duration: 100 })
            .addEventListener('finish', (e) => {
              rsltWindow.replaceChildren();
              rsltWindow.insertAdjacentHTML('afterbegin', data);
              rsltWindow.animate({ opacity: [0, 1] }, { duration: 100, fill: "forwards" });

              const
                deleteBtns = rsltWindow.querySelectorAll('[data-registry-delete]'),
                editBtns = rsltWindow.querySelectorAll('[data-registry-edit]');

              deleteBtns.forEach(btn => btn.addEventListener('click', events.onDelete_Click));
              editBtns.forEach(btn => btn.addEventListener('click', events.onEdit_Click))
            });

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
    onSave_Click: () => {
      const
        endLoading = Loading(),
        id = getInputValue('[data-add-id]'),
        name = getInputValue('[data-add-name]'),
        description = getInputValue('[data-add-description]'),
        notes = getInputValue('[data-add-notes]'),
        eventDate = getInputValue('[data-add-event-date]'),
        url = (+id > 0) ? _updateRoute : _insertRoute;

      try {
        const
          success = Post(url, {
            id,
            name,
            description,
            eventDate,
            notes
          });

        if (success) {
          const dialog = querySelector('[data-modal-add]');
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
        { Id, Name, EventDate, Description, Notes } = data,
        modal = prepareModal(Name, Description, EventDate, Notes, Id);

      modal.showModal();
    }
  };

export { init }