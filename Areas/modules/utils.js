const
  formatter = new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD'
  });

const
  LogError = (str, data) => console.debug(str, data),
  IsEmpty = (arr) => {
    if (!Array.isArray(arr)) {
      return false;
    }

    return arr.length === 0;
  },
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

    return els;
  },
  Get = (url, params) => {
    return new Promise((resolve, _) => {
      $.ajax({
        type: "GET",
        data: params,
        url,
        success: (data) => resolve({ success: true, data }),
        error: (_, status, error) => resolve({ success: false, data: null, error: error })
      });
    });
  },
  GetFile = (url, params) => {
    return new Promise((resolve, _) => {
      $.ajax({
        type: "GET",
        data: params,
        url,
        xhrFields: {
          responseType: 'blob'
        },
        success: (data) => resolve({ success: true, data }),
        error: (_, status, error) => resolve({ success: false, data: null, error })
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
        error: (_, status, error) => resolve({ success: false, data: null, error: error })
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
        error: (_, status, error) => resolve({ success: false, data: null, error: error })
      });
    });
  },
  Loading = () => {
    const
      el = QuerySelector('[data-template-loading]'),
      body = document.querySelector('dialog[open]') ?? QuerySelector('[data-registry]'),
      clone = el.content.cloneNode(true);

    body.appendChild(clone);

    return () => {
      const
        duration = 200,
        loading = QuerySelector('[data-loader]');

      loading.animate([{ opacity: 0 }], { duration, fill: "forwards" });
      setTimeout(() => loading.remove(), duration + 3);
    }
  },
  UseTemplateTag = (templateSelector, containerSelector, elSelector) => {
    const
      el = QuerySelector(templateSelector),
      container = QuerySelector(containerSelector),
      clone = el.content.cloneNode(true);

    container.appendChild(clone);

    const component = container.querySelector(elSelector);

    return {
      component,
      onRemove: (duration = 200) => {
        component.animate([{ opacity: 0 }], { duration, fill: "forwards" });
        setTimeout(() => {
          component.remove();
          return Promise.resolve();
        }, duration - 3);
      }
    }
  },
  GetInputValue = (selector, parent, options = { isNumeric: false, isBoolean: false }) => {
    const
      { isNumeric, isBoolean } = options,
      input = QuerySelector(selector, parent);

    if (!input) {
      LogError(`no element with the selector`, { selector, parent });
      return isNumeric ? 0 : isBoolean ? false : "";
    }

    if (isNumeric) {
      return Number(input.value)
    }

    if (isBoolean) {
      return Boolean(input.value);
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

    if ('value' in input) {
      input.value = value;
      return;
    }

    input.innerHTML = value;
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
  DisplayNotification = async (val, useBody = false) => {
    const
      body = useBody ? document.body : document.querySelector('dialog[open]') ?? document.body,
      clone = document
        .querySelector('[data-registry-notify-template]')
        .content
        .cloneNode(true);

    body.appendChild(clone);

    const
      notification = document.querySelector('[data-registry-notify]'),
      main = notification.querySelector('main');

    main.innerText = val;

    await FadeIn(notification);
    await Delay(2000);
    await FadeOut(notification);

    notification.remove();
  },
  AddQueryParamToURL = (arr) => {

    if (!Array.isArray(arr)) {
      return;
    }

    try {
      const
        url = new URL(window.location.href),
        urlParam = new URLSearchParams(url.search);

      arr.forEach(({ key, value }) => {
        if (!value?.trim()) {
          urlParam.delete(key);
          return;
        }

        urlParam.set(key, value)
      });

      window.history.replaceState(null, null, `?${urlParam.toString()}`);

    } catch (error) {
      LogError("URL param set", error);
    }
  },
  GetQueryParam = (key) => {
    try {
      const
        url = new URL(window.location.href),
        urlParam = new URLSearchParams(url.search);

      return urlParam.get(key);

    } catch (error) {
      LogError("URL param get", error);
    }
  },
  GetDataSet = (event) => ({ ...event?.target?.dataset ?? {} }),
  ToCurrency = (val) => formatter.format(val),
  FadeOut = (el, duration = 200) => new Promise((res) => {
    el.animate({ opacity: [1, 0] }, { duration, fill: "forwards" })
      .addEventListener('finish', (e) => res(() => FadeIn(el, duration)));
  }),
  FadeIn = (el, duration = 200) => new Promise((res) => {
    el.animate({ opacity: [0, 1] }, { duration, fill: "forwards" })
      .addEventListener('finish', (e) => res());
  }),
  Delay = (milisecond = 300) => new Promise((res) => setTimeout(() => res(), milisecond)),
  SaveAsFile = (data, fileName) => {
    const
      a = document.createElement('a'),
      url = window.URL.createObjectURL(data);

    a.href = url;
    a.download = fileName;
    document.body.append(a);
    a.click();
    a.remove();
    window.URL.revokeObjectURL(url);
  },
  NopDateControlValue = (selector) => {
    const
      day = GetInputValue(`select[name=${selector}_day]`),
      month = GetInputValue(`select[name=${selector}_month]`),
      year = GetInputValue(`select[name=${selector}_year]`),
      val = `${day}/${month}/${year}`,
      isDate = val instanceof Date && !isNaN(val);

    return { value: isDate ? val : undefined };
  }

export {
  Get,
  Post,
  Delete,
  LogError,
  QuerySelector,
  QuerySelectorAll,
  Loading,
  GetInputValue,
  SetInputValue,
  DateToInputString,
  DisplayNotification,
  AddQueryParamToURL,
  GetQueryParam,
  UseTemplateTag,
  GetDataSet,
  ToCurrency,
  FadeOut,
  IsEmpty,
  GetFile,
  SaveAsFile,
  NopDateControlValue
}