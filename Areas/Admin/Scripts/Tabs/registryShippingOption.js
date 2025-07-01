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
            btnClose = _main.querySelector(':scope [data-registry-shipping-close]'),
            btnSave = _main.querySelector(':scope [data-registry-shipping-submit]'),
            btnNew = _main.querySelector(':scope [data-shipping-option-add]');

        btnClose.addEventListener('click', events.onDialog_Close);
        btnSave.addEventListener('click', events.onSave_Click);
        btnNew.addEventListener('click', events.onNewOption_Click);
    },
    setupRowEvents = () => {
        const
            editBtns = document.querySelectorAll('[data-edit-shipping-option]'),
            dltBtns = document.querySelectorAll('[data-delete-shipping-option]');

        editBtns.forEach(btn => btn.addEventListener('click', events.onEditRow_Click));
        dltBtns.forEach(btn => btn.addEventListener('click', events.onDeletedRow_Click));
    },
    showEditDialog = (id = '', name = '', description = '') => {
        const
            dialog = _main.querySelector('dialog');

        SetInputValue('[data-registry-shipping-id]', '[data-dialog-edit]', id);
        SetInputValue('[data-registry-shipping-name]', '[data-dialog-edit]', name);
        SetInputValue('[data-registry-shipping-description]', '[data-dialog-edit]', description);

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
                id = GetInputValue('[data-registry-shipping-id]', '[data-dialog-edit]'),
                name = GetInputValue('[data-registry-shipping-name]', '[data-dialog-edit]'),
                description = GetInputValue('[data-registry-shipping-description]', '[data-dialog-edit]');

            await Post(_url, { id, name, description });
            onChangeEvent(e.target, "ship");
        },
        onDialog_Close: () => {
            const dialog = _main.querySelector('dialog');

            dialog.close();
        },
        onEditRow_Click: (e) => {
            const
                { id, name, description } = GetDataSet(e);

            showEditDialog(id, name, description);
        },
        onDeletedRow_Click: async (e) => {
            const
                { id, name, description } = GetDataSet(e);

            await Post(_url, { id, name, description, deleted: true });
            onChangeEvent(e.target, "ship");
        }
    }

export { init }