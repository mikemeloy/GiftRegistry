const
  Log = (str) => console.log(str),
  LogError = (str, data) => console.error(str, data),
  QuerySelector = (selector = "", parent = "body") => {
    const
      container = document.querySelector(parent),
      el = container.querySelector(`:scope ${selector}`);

    if (!el) {
      console.error(`element with the selector of ${selector} not found!`);
    }

    return el ?? {
      showModal: () => console.log(`method doesn't exist!`)
    };
  },
  QuerySelectorAll = (selector = "", parent = "") => {
    const
      container = document.querySelector(parent),
      els = container.querySelectorAll(`:scope ${selector}`);

    return els ?? [];
  },
  Get = (url, params) => {
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
  Post = (url, params) => {
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
  },
  Delete = (url, params) => {
    return new Promise((resolve, _) => {
      $.ajax({
        type: "DELETE",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: params,
        url,
        success: (data) => resolve({ success: true, data }),
        error: (_, status, errorThrown) => resolve({ success: false, data: null, error: errorThrown })
      });
    });
  },
  Loading = () => {
    const
      el = QuerySelector('[data-template-loading]'),
      body = QuerySelector('[data-registry]'),
      clone = el.content.cloneNode(true);

    body.appendChild(clone);

    return () => {
      const
        duration = 200,
        loading = QuerySelector('[data-loader]');

      loading.animate([{ opacity: 0 }], { duration });
      setTimeout(() => loading.remove(), duration - 3);
    }
  },
  GetInputValue = (selector, parent) => {
    const
      input = QuerySelector(selector, parent);

    if (!input) {
      return "";
    }

    return input.value;
  },
  SetInputValue = (selector, parent, value) => {
    const
      input = QuerySelector(selector, parent);

    if (!input) {
      console.error(`input selector [${parent} ${selector}] not found`)
      return;
    }

    input.value = value;
  },
  DateToInputString = (val) => {
    try {
      const
        seed = new Date(val),
        year = seed.toLocaleString('default', { year: 'numeric' }),
        month = seed.toLocaleString('default', { month: '2-digit' }),
        day = seed.toLocaleString('default', { day: '2-digit' });

      return [year, month, day].join('-');
    } catch (error) {
      console.error(error);
    }

    return new Date().toLocaleDateString();
  },
  DisplayNotification = (val) => {
    const
      clone = document
        .querySelector('[data-registry-notify-template]')
        .content
        .cloneNode(true),
      options = { duration: 1000, fill: "forwards" };

    document.body.appendChild(clone);

    const
      dialog = document.querySelector('[data-registry-notify]'),
      main = dialog.querySelector('main');

    main.innerText = val;

    dialog
      .animate({ opacity: [0, 1] }, options)
      .addEventListener('finish', (e) => {
        setTimeout(() => {
          dialog
            .animate({ opacity: [1, 0] }, options)
            .addEventListener('finish', (e) => { dialog.remove() });
        }, options.duration);
      })
  };

export {
  Get,
  Post,
  Delete,
  Log,
  LogError,
  QuerySelector,
  QuerySelectorAll,
  Loading,
  GetInputValue,
  SetInputValue,
  DateToInputString,
  DisplayNotification
}