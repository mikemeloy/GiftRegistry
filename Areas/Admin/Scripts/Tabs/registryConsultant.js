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
            submit = _main.querySelector(':scope [data-registry-consultant-submit]');

        submit.addEventListener('click', events.onAddConsultant_Click);
    };

const events = {
    onAddConsultant_Click: async (e) => {
        e.preventDefault();

        const
            name = _main.querySelector(':scope [data-registry-consultant-name]'),
            email = _main.querySelector(':scope [data-registry-consultant-email]');

        await Post(_url, { name: name.value, email: email.value })
    }
}


export { init }