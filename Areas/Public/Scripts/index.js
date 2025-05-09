let
  _container,
  _searchUrl,
  _insertRoute;

const
  init = (searchUrl, insertRoute) => {
    _searchUrl = searchUrl;
    _insertRoute = insertRoute;
    setFormEvents();
  },
  setFormEvents = () => {
    const
      form = utils.querySelector('[data-search]'),
      btnAdd = utils.querySelector('[data-add]'),
      btnSave = utils.querySelector('[data-registry-save]');

    form.addEventListener('submit', events.onSearch_Submit);
    btnAdd.addEventListener('click', events.onAdd_Click);
    btnSave.addEventListener('click', events.onSave_Click);
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
  },
  get: (url, params) => {
    return new Promise((resolve, _) => {
      $.ajax({
        type: "GET",
        data: params,
        url,
        success: (data) => resolve({ success: true, data }),
        error: (_, status, errorThrown) => resolve({ success: false, data: null, error: errorThrown })
      });
    });
  },
  post: (url, params) => {
    return new Promise((resolve, _) => {
      $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(params),
        url,
        success: (data) => resolve({ success: true, data }),
        error: (_, status, errorThrown) => resolve({ success: false, data: null, error: errorThrown })
      });
    });
  }
}

const events = {
  onSearch_Submit: async (e) => {
    e.preventDefault();

    const
      query = utils.getInputValue('[data-search] input'),
      { success, data, error } = await utils.get(_searchUrl, { query }),
      rsltWindow = utils.querySelector('[data-result]');

    if (!success) {
      console.error(error);
      return;
    }

    rsltWindow.insertAdjacentHTML('afterbegin', data);
  },
  onAdd_Click: () => {
    const
      modal = utils.querySelector('[data-modal-add]');

    modal.showModal();
  },
  onSave_Click: () => {
    const
      name = utils.getInputValue('[data-add-name]'),
      description = utils.getInputValue('[data-add-description]'),
      eventDate = utils.getInputValue('[data-add-event-date]');

    utils.post(_insertRoute, {
      name,
      description,
      eventDate
    });
  }
}

export { init }