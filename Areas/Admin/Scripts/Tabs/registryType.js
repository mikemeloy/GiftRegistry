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
            submit = _main.querySelector(':scope [data-registry-type-submit]');

        submit.addEventListener('click', events.onAddRegistryType_Click);
    };

const events = {
    onAddRegistryType_Click: async (e) => {
        e.preventDefault();

        const
            name = _main.querySelector(':scope [data-registry-type-name]'),
            description = _main.querySelector(':scope [data-registry-type-description]');

        await Post(_url, { name: name.value, description: description.value })
    }
}

export { init }