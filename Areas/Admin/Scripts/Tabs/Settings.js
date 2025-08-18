import { Post, GetDataSet, SetInputValue, GetInputValue } from '../../../modules/utils.js';

let
    _main,
    _url;

const
    init = (el, url) => {
        _main = el;
        _url = url;

        setupFormEvents();
    },
    setupFormEvents = () => {
        const
            btnSubmit = _main.querySelector(':scope [data-settings-submit]');

        btnSubmit.addEventListener('click', events.onSubmit_Click);
    };

const events = {
    onSubmit_Click: async (e) => {
        const
            productKey = GetInputValue('[data-settings-product-key]', '[data-registry-admin-main]');

        await Post(_url, { productKey });
    }
};

export { init }