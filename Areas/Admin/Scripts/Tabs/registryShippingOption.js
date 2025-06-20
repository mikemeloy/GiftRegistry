import { Post } from '../../../modules/utils.js';

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
            submit = _main.querySelector(':scope [data-registry-shipping-submit]');

        submit.addEventListener('click', events.onAddShippingOption_Click);
    };

const events = {
    onAddShippingOption_Click: async (e) => {
        e.preventDefault();

        const
            name = _main.querySelector(':scope [data-registry-shipping-name]'),
            description = _main.querySelector(':scope [data-registry-shipping-email]');

        await Post(_url, { name: name.value, description: description.value })
    }
}


export { init }