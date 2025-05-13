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
        loading = QuerySelector('[data-loader]');

      body.removeChild(loading);
    }
  };

export {
  Get,
  Post,
  Delete,
  Log,
  LogError,
  QuerySelector,
  QuerySelectorAll,
  Loading
}