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
            btnClose = _main.querySelector(':scope [data-registry-type-close]'),
            btnSave = _main.querySelector(':scope [data-registry-type-submit]'),
            btnNew = _main.querySelector(':scope [data-type-option-add]');

        btnClose.addEventListener('click', events.onDialog_Close);
        btnSave.addEventListener('click', events.onSave_Click);
        btnNew.addEventListener('click', events.onNewOption_Click);
    },
    setupRowEvents = () => {
        const
            editBtns = document.querySelectorAll('[data-edit-type-option]'),
            dltBtns = document.querySelectorAll('[data-delete-type-option]');

        editBtns.forEach(btn => btn.addEventListener('click', events.onEditRow_Click));
        dltBtns.forEach(btn => btn.addEventListener('click', events.onDeletedRow_Click));
    },
    showEditDialog = (id = '', name = '', description = '', order = 0) => {
        const
            dialog = _main.querySelector('dialog');

        SetInputValue('[data-registry-type-id]', '[data-dialog-edit]', id);
        SetInputValue('[data-registry-type-name]', '[data-dialog-edit]', name);
        SetInputValue('[data-registry-type-order]', '[data-dialog-edit]', order);
        SetInputValue('[data-registry-type-description]', '[data-dialog-edit]', description);

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
                id = GetInputValue('[data-registry-type-id]', '[data-dialog-edit]'),
                name = GetInputValue('[data-registry-type-name]', '[data-dialog-edit]'),
                description = GetInputValue('[data-registry-type-description]', '[data-dialog-edit]'),
                sortOrder = GetInputValue('[data-registry-type-order]', '[data-dialog-edit]');

            await Post(_url, { id, name, description, sortOrder });
            onChangeEvent(e.target, "type");
        },
        onDialog_Close: () => {
            const dialog = _main.querySelector('dialog');

            dialog.close();
        },
        onEditRow_Click: (e) => {
            const
                { id, name, description, order } = GetDataSet(e);

            showEditDialog(id, name, description, order);
        },
        onDeletedRow_Click: async (e) => {
            const
                { id, name, description, order } = GetDataSet(e);

            await Post(_url, { id, name, description, order, deleted: true });
            onChangeEvent(e.target, "type");
        }
    }

export { init }