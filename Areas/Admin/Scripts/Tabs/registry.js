import {
    Get, AddQueryParamToURL, QuerySelector,
    LogError, GetInputValue, GetDataSet,
    FadeOut
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
    },
    querySelector = (selector) => QuerySelector(selector, '[data-registry]'),
    getInputValue = (selector, options) => GetInputValue(selector, '[data-registry]', options),
    setFormEvents = () => {
        const
            form = querySelector('[data-search]');

        form.addEventListener('keyup', events.onSearch_KeyUp);
    },
    debounce = (el) => {
        clearTimeout(_debounce);
        _debounce = setTimeout(async () => await el?.(), 400);
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
                        editBtns = rsltWindow.querySelectorAll('[data-action-edit]'),
                        viewBtns = rsltWindow.querySelectorAll('[data-action-view]');

                    editBtns.forEach(btn => btn.addEventListener('click', events.onEdit_Click));
                    viewBtns.forEach(btn => btn.addEventListener('click', events.onView_Click));

                    await onComplete();

                } catch (error) {
                    LogError('Failed to Search', error);
                }
            });
        },
        onView_Click: (e) => {
            const
                { registryId } = GetDataSet(e);
        },
        onEdit_Click: (e) => {
            const
                { registryId } = GetDataSet(e),
                dialog = _main.querySelector('[data-dialog-edit-registry]');

            dialog.showModal();
        }
    }

export { init }