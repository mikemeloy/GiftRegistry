import { Get, Post, Delete, Loading, LogError } from './modules/utils.js';

let
  _container,
  _searchRoute,
  _insertRoute,
  _deleteRoute,
  _debounce;

const
  init = (searchUrl, insertRoute, deleteRoute) => {
    _searchRoute = searchUrl;
    _insertRoute = insertRoute;
    _deleteRoute = deleteRoute;
    setFormEvents();
  },
  setFormEvents = () => {
    const
      form = utils.querySelector('[data-search]'),
      btnAdd = utils.querySelector('[data-add]'),
      btnSave = utils.querySelector('[data-registry-save]');

    form.addEventListener('keyup', events.onSearch_KeyUp);
    btnAdd.addEventListener('click', events.onAdd_Click);
    btnSave.addEventListener('click', events.onSave_Click);
  },
  debounce = (el) => {
    clearTimeout(_debounce);
    _debounce = setTimeout(async () => await el?.(), 400);
  }


const utils = {
  querySelector: (selector) => {
    if (!_container) {
      _container = document.querySelector('[data-registry]');
    }

    const
      el = _container.querySelector(`:scope ${selector}`);

    if (!el) {
      console.error(`element with the selector of ${selector} not found!`);
    }

    return el ?? {
      showModal: () => console.log(`method doesn't exist!`)
    };
  },
  getInputValue: (selector) => {
    const
      input = utils.querySelector(selector);

    if (!input) {
      return "";
    }

    return input.value;
  }
}

const events = {
  onSearch_KeyUp: async () => {
    debounce(async () => {
      const
        endLoading = Loading();

      try {
        const
          query = utils.getInputValue('[data-search] input'),
          { success, data, error } = await Get(_searchRoute, { query }),
          rsltWindow = utils.querySelector('[data-result]');

        if (!success) {
          LogError('Failed to Search', error);
          return;
        }

        rsltWindow.replaceChildren();
        rsltWindow.insertAdjacentHTML('afterbegin', data);

        const
          btns = rsltWindow.querySelectorAll('[data-registry-id]');

        btns.forEach(btn => btn.addEventListener('click', events.onDelete_Click));
      } catch (error) {
        LogError('Failed to Search', error);
      } finally {
        endLoading();
      }
    });
  },
  onAdd_Click: () => {
    const
      modal = utils.querySelector('[data-modal-add]');

    modal.showModal();
  },
  onSave_Click: () => {
    const
      endLoading = Loading(),
      name = utils.getInputValue('[data-add-name]'),
      description = utils.getInputValue('[data-add-description]'),
      notes = utils.getInputValue('[data-add-notes]'),
      eventDate = utils.getInputValue('[data-add-event-date]');

    try {
      const
        success = Post(_insertRoute, {
          name,
          description,
          eventDate,
          notes
        });

      if (success) {
        const dialog = utils.querySelector('[data-modal-add]');
        dialog.close();
      }
    } catch (error) {
      LogError(error);
    } finally {
      endLoading();
    }
  },
  onDelete_Click: async ({ target }) => {
    const
      endLoading = Loading(),
      { dataset: { registryId } } = target;

    try {
      const
        success = await Delete(`${_deleteRoute}?id=${registryId}`);

      if (success) {
        events.onSearch_KeyUp();
      }
    } catch (error) {
      LogError(error);
    } finally {
      endLoading();
    }
  }
}

export { init }