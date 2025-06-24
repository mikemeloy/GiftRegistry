import { Post, GetDataSet, SetInputValue, GetInputValue } from '../../../modules/utils.js';

let
    _main,
    _url;

const
    init = (el, url) => {
        _main = el;
        _url = url;
        setupFormEvents();
        setupRowEvents();
    },
    setupFormEvents = () => {
        const
            btnClose = _main.querySelector(':scope [data-registry-consultant-close]'),
            btnSave = _main.querySelector(':scope [data-registry-consultant-submit]'),
            btnNew = _main.querySelector(':scope [data-consultant-option-add]');

        btnClose.addEventListener('click', events.onDialog_Close);
        btnSave.addEventListener('click', events.onSave_Click);
        btnNew.addEventListener('click', events.onNewOption_Click);
    },
    setupRowEvents = () => {
        const
            editBtns = document.querySelectorAll('[data-edit-consultant-option]'),
            dltBtns = document.querySelectorAll('[data-delete-consultant-option]');

        editBtns.forEach(btn => btn.addEventListener('click', events.onEditRow_Click));
        dltBtns.forEach(btn => btn.addEventListener('click', events.onDeletedRow_Click));
    },
    showEditDialog = (id = '', name = '', email = '') => {
        const
            dialog = _main.querySelector('dialog');

        SetInputValue('[data-registry-consultant-id]', '[data-dialog-edit]', id);
        SetInputValue('[data-registry-consultant-name]', '[data-dialog-edit]', name);
        SetInputValue('[data-registry-consultant-email]', '[data-dialog-edit]', email);

        dialog.showModal();
    },
    onChangeEvent = (el, detail = "home") => {
        el?.dispatchEvent(new CustomEvent("refresh", {
            bubbles: true,
            detail
        }));
    };

const
    events = {
        onNewOption_Click: () => {
            const dialog = _main.querySelector('dialog');

            showEditDialog();

            dialog.showModal();
        },
        onSave_Click: async (e) => {
            const
                id = GetInputValue('[data-registry-consultant-id]', '[data-dialog-edit]'),
                name = GetInputValue('[data-registry-consultant-name]', '[data-dialog-edit]'),
                email = GetInputValue('[data-registry-consultant-email]', '[data-dialog-edit]');

            await Post(_url, { id, name, email });
            onChangeEvent(e.target, "consultant");
        },
        onDialog_Close: () => {
            const dialog = _main.querySelector('dialog');

            dialog.close();
        },
        onEditRow_Click: (e) => {
            const
                { id, name, email } = GetDataSet(e);

            showEditDialog(id, name, email);
        },
        onDeletedRow_Click: async (e) => {
            const
                { id, name, email } = GetDataSet(e);

            await Post(_url, { id, name, email, deleted: true });
            onChangeEvent(e.target, "consultant");
        }
    }

export { init }