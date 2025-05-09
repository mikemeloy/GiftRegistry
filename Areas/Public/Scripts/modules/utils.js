let
  _container;

const
  Log = (str) => console.log(str),
  QuerySelector = (selector = "", parent = "") => {
    if (!_container) {
      _container = document.querySelector(parent);
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
  }


export { Get, Post, Log, QuerySelector }