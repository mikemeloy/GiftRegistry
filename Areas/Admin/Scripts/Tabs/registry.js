import {
    AddQueryParamToURL, QuerySelector,
    LogError, GetInputValue, GetDataSet,
    FadeOut, Post, Get,
    SetInputValue, GetQueryParam
} from '../../../modules/utils.js';

let
    _main,
    _url,
    _debounce;

const
    init = async (el, url) => {
        _main = el;
        _url = url;
        setFormEvents();
        setSearchByUrl();
    },
    setSearchByUrl = () => {
        const
            searchParam = GetQueryParam('search'),
            searchInput = querySelector('form input');

        if (!searchParam || !searchInput) {
            return;
        }

        searchInput.value = searchParam;
        events.onSearch_KeyUp();
    },
    querySelector = (selector) => QuerySelector(selector, '[data-registry]'),
    getInputValue = (selector, options) => GetInputValue(selector, '[data-registry]', options),
    setInputValue = (selector, value) => SetInputValue(selector, '[data-registry]', value),
    setFormEvents = () => {
        const
            form = querySelector('[data-search]'),
            save = querySelector('[data-admin-update-save]'),
            close = querySelector('[data-dialog-close]');

        form.addEventListener('keyup', events.onSearch_KeyUp);
        save.addEventListener('click', events.onSave_Click);
        close.addEventListener('click', events.onClose_Click)
    },
    debounce = (el) => {
        clearTimeout(_debounce);
        _debounce = setTimeout(async () => await el?.(), 400);
    },
    setEditorFields = async (dialog, registryId, registryName) => {
        const
            header = dialog.querySelector('header h3'),
            setValue = (key, value) => setInputValue(`[data-admin-update-${key}]`, value),
            { success, data } = await Get(`${_url}/Get?id=${registryId}`);

        if (!success) {
            return false;
        }

        const {
            DeliveryMethodId,
            EventTypeId,
            AdminNotes,
            ConsultantId
        } = data;

        setValue('header', registryName);
        setValue('id', registryId);
        setValue('notes', AdminNotes);
        setValue('shipping', DeliveryMethodId);
        setValue('consultant', ConsultantId);
        setValue('type', EventTypeId);

        return true;
    };

const
    events = {
        onSearch_KeyUp: async () => {
            debounce(async () => {
                try {
                    const
                        query = getInputValue('[data-search] input'),
                        { success, data, error } = await Get(`${_url}/Query`, { query }),
                        rsltWindow = querySelector('[data-result]');

                    if (!success) {
                        LogError('Failed to Search', error);
                        return;
                    }

                    AddQueryParamToURL([{ key: 'search', value: query }]);

                    const
                        onComplete = await FadeOut(rsltWindow);

                    rsltWindow.replaceChildren();
                    rsltWindow.insertAdjacentHTML('afterbegin', data);

                    const
                        editBtns = rsltWindow.querySelectorAll('[data-action-edit]');

                    editBtns.forEach(btn => btn.addEventListener('click', events.onEdit_Click));

                    await onComplete();

                } catch (error) {
                    LogError('Failed to Search', error);
                }
            });
        },
        onEdit_Click: async (e) => {
            const
                { registryId, registryName } = GetDataSet(e),
                dialog = _main.querySelector('[data-dialog-edit-registry]'),
                onFadeComplete = await FadeOut(dialog),
                finished = setEditorFields(dialog, registryId, registryName);

            dialog.showModal();

            await onFadeComplete();
        },
        onSave_Click: async (e) => {
            const
                dialog = _main.querySelector('[data-dialog-edit-registry]'),
                getValueFor = (selector, options) => getInputValue(`[data-admin-update-${selector}]`, options),
                id = getValueFor('id', { isNumeric: true }),
                notes = getValueFor('notes'),
                shipping = getValueFor('shipping', { isNumeric: true }),
                consultant = getValueFor('consultant', { isNumeric: true }),
                eventType = getValueFor('type', { isNumeric: true });

            await Post(_url, { id, notes, shipping, consultant, eventType });

            await FadeOut(dialog);
            dialog.close();
        },
        onClose_Click: async ({ target }) => {
            const
                dialog = target.closest('dialog'),
                onFadeComplete = await FadeOut(dialog);

            dialog?.close();
            await onFadeComplete();
        }
    }

export { init }