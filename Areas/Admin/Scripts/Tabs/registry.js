import {
    AddQueryParamToURL, QuerySelector,
    LogError, GetInputValue, GetDataSet,
    FadeOut, Post, Get, DateToInputString,
    SetInputValue, GetQueryParam,
    DisplayNotification, Delete,
    IsEmpty, ToCurrency, UseTemplateTag,
    SaveAsFile, GetFile
} from '../../../modules/utils.js';

let
    _main,
    _url,
    _deleteUrl,
    _reportUrl,
    _debounce;

const
    init = async (el, url, deleteUrl, reportUrl) => {
        _main = el;
        _url = url;
        _deleteUrl = deleteUrl;
        _reportUrl = reportUrl;

        setFormEvents();
        setSearchByUrl();
    },
    setSearchByUrl = () => {
        const
            searchParam = GetQueryParam('search'),
            searchInput = querySelector('form input');

        searchInput.focus();

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
            report = querySelector('[data-action-report]'),
            form = querySelector('[data-search]'),
            save = querySelector('[data-admin-update-save]'),
            close = querySelector('[data-dialog-close]');

        form.addEventListener('keyup', events.onSearch_KeyUp);
        save.addEventListener('click', events.onSave_Click);
        close.addEventListener('click', events.onClose_Click);
        report.addEventListener('click', events.onReport_Click)
    },
    debounce = (el) => {
        clearTimeout(_debounce);
        _debounce = setTimeout(async () => await el?.(), 400);
    },
    setEditorFields = async (registryId, registryName) => {
        const
            setValue = (key, value) => setInputValue(`[data-admin-update-${key}]`, value),
            { success, data } = await Get(`${_url}/Get?id=${registryId}`);

        if (!success) {
            return false;
        }

        const {
            DeliveryMethodId,
            EventTypeId,
            AdminNotes,
            ConsultantId,
            RegistryItems,
            RegistryOrders,
            Name,
            ClientNotes,
            EventDate,
            Sponsor,
            Summary
        } = data;

        setValue('header', registryName);
        setValue('id', registryId);
        setValue('admin-notes', AdminNotes);
        setValue('shipping', DeliveryMethodId);
        setValue('consultant', ConsultantId);
        setValue('type', EventTypeId);
        setValue('name', Name);
        setValue('summary', Summary);
        setValue('event-date', DateToInputString(EventDate));
        setValue('sponsor', Sponsor);
        setValue('client-notes', ClientNotes);

        generateItemRow(RegistryItems);
        generateOrderRow(RegistryOrders);

        return true;
    },
    generateOrderRow = (orders) => {
        const
            container = querySelector('[data-registry-order]');

        if (IsEmpty(orders)) {
            container.replaceChildren();
            return;
        }

        const
            headerTemplate = querySelector('[data-template-registry-order-header]'),
            headerClone = headerTemplate.content.cloneNode(true),
            rowTemplate = querySelector('[data-template-registry-order-row]');

        container.replaceChildren(headerClone);

        for (const order of orders) {
            const
                clone = rowTemplate.content.cloneNode(true),
                { RegistryId, OrderId, OrderTotal, FullName } = order,
                setValue = (selector, value, fn) => {
                    const el = clone.querySelector(`[data-${selector}]`);

                    if (!el) {
                        return;
                    }

                    el.dataset.rowId = RegistryId;
                    el.innerHTML = value;

                    if (fn) {
                        el.addEventListener('click', fn);
                    }

                    return el;
                };

            setValue('order-id', OrderId, () => window.open(`${location.origin}/Admin/Order/Edit/${OrderId}`, '_blank'));
            setValue('order-total', ToCurrency(OrderTotal));
            setValue('order-customer', FullName);

            container.appendChild(clone);
        }
    },
    generateItemRow = (registryItems) => {
        const
            container = querySelector('[data-registry-item]');

        if (IsEmpty(registryItems)) {
            container.replaceChildren()
            return;
        }

        const
            headerTemplate = querySelector('[data-template-registry-item-header]'),
            headerClone = headerTemplate.content.cloneNode(true),
            rowTemplate = querySelector('[data-template-registry-item-row]');

        container.replaceChildren(headerClone)

        for (const registryItem of registryItems) {
            const
                { Id, Name, Price, Quantity, Purchased } = registryItem,
                clone = rowTemplate.content.cloneNode(true),
                btnDelete = clone.querySelector('[data-action-delete]'),
                btnEdit = clone.querySelector('[data-action-edit]'),
                actionRow = clone.querySelector('[data-actions]'),
                setValue = (selector, value) => {
                    const el = clone.querySelector(`[data-${selector}]`);

                    if (!el) {
                        return;
                    }

                    el.dataset.rowId = Id;
                    el.innerHTML = value;

                    return el;
                };

            const
                cellQuantity = setValue('quantity', Quantity);

            setValue('name', Name);
            setValue('price', ToCurrency(Price));
            setValue('purchased', Purchased);

            actionRow.dataset.rowId = Id;
            cellQuantity.dataset.min = Purchased;

            btnEdit.addEventListener('click', () => events.onRegistryitemEdit_Click(registryItem));
            btnDelete.addEventListener('click', () => events.onRegistryitemDelete_Click(registryItem));

            container.appendChild(clone);
        }
    },
    getRowCell = (cellName = '', id = 0) => {
        const
            container = querySelector('[data-registry-item]');

        return container.querySelector(`[data-${cellName}][data-row-id="${id}"]`);
    },
    getReportParams = async () => {
        const
            { component: dialog, onRemove } = UseTemplateTag(
                '[data-template-report-dialog]',
                '[data-registry]',
                '[data-dialog-report]'
            ),
            name = dialog.querySelector(':scope [data-report-query-name]'),
            start = dialog.querySelector(':scope [data-report-query-start-date]'),
            end = dialog.querySelector(':scope [data-report-query-end-date]'),
            submit = dialog.querySelector(':scope [data-dialog-report-submit]'),
            onFadeComplete = await FadeOut(dialog);

        dialog.showModal();

        onFadeComplete();

        return new Promise((res) => submit.addEventListener('click', () => res({
            params: {
                name: name.value,
                startDate: start.value,
                endDate: end.value
            },
            closeParamDialog: onRemove
        })));
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
                        deleteBtns = rsltWindow.querySelectorAll('[data-admin-update-deleted] input');

                    editBtns.forEach(btn => btn.addEventListener('click', events.onEdit_Click));
                    deleteBtns.forEach(btn => btn.addEventListener('change', events.onDelete_Change))

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
                success = setEditorFields(registryId, registryName);

            if (!success) {
                DisplayNotification("Unable to Edit Registry");
                return;
            }

            const
                onFadeComplete = await FadeOut(dialog);

            dialog.showModal();

            await onFadeComplete();
        },
        onSave_Click: async (e) => {
            const
                dialog = _main.querySelector('[data-dialog-edit-registry]'),
                getValueFor = (selector, options) => getInputValue(`[data-admin-update-${selector}]`, options),
                id = getValueFor('id', { isNumeric: true }),
                name = getValueFor('name'),
                summary = getValueFor('summary'),
                adminNotes = getValueFor('admin-notes'),
                deliveryMethodId = getValueFor('shipping', { isNumeric: true }),
                consultantId = getValueFor('consultant', { isNumeric: true }),
                eventTypeId = getValueFor('type', { isNumeric: true }),
                clientNotes = getValueFor('client-notes'),
                sponsor = getValueFor('sponsor'),
                eventDate = getValueFor('event-date');

            await Post(_url, {
                id,
                name,
                summary,
                adminNotes,
                deliveryMethodId,
                consultantId,
                eventTypeId,
                clientNotes,
                sponsor,
                eventDate
            });

            await FadeOut(dialog);
            dialog.close();
        },
        onDelete_Change: async (e) => {
            const
                { registryId } = GetDataSet(e),
                success = await Delete(`${_url}?id=${registryId}`),
                notification = success ? 'Saved' : 'An Error Has Occurred';

            DisplayNotification(notification);
        },
        onClose_Click: async ({ target }) => {
            const
                dialog = target.closest('dialog'),
                onFadeComplete = await FadeOut(dialog);

            dialog?.close();
            await onFadeComplete();
        },
        onRegistryitemDelete_Click: async ({ Id, Purchased }) => {
            if (Purchased > 0) {
                DisplayNotification(`Item Has Already Been Purchased and Cannot be Deleted!`);
                return;
            }

            const
                { success, data: deleted } = await Delete(`${_deleteUrl}?id=${Id}`);

            if (!success || !deleted) {
                DisplayNotification(`Unable to Delete Item From Registry`);
                return;
            }

            const
                rows = document.querySelectorAll(`[data-row-id="${Id}"]`),
                actions = document.querySelector(`[data-actions][data-row-id="${Id}"]`);

            for (const el of rows) {
                el.animate({ opacity: [1, .2] }, { duration: 300, fill: "forwards" });
            }

            actions?.remove();
        },
        onRegistryitemEdit_Click: (registryItem) => {
            const
                { Id, Purchased } = registryItem,
                quantityCell = getRowCell('quantity', Id),
                value = quantityCell.innerHTML;

            if (!quantityCell) {
                console.error(`Edit Cell not found`);
                return;
            }

            quantityCell.contentEditable = true;
            quantityCell.focus();

            quantityCell.onblur = async () => {
                const
                    newQuantity = quantityCell.innerHTML,
                    lessThanPurchased = (newQuantity < Purchased);

                quantityCell.contentEditable = false;

                if (isNaN(newQuantity) || lessThanPurchased) {
                    quantityCell.innerHTML = value;

                    DisplayNotification(`Please enter a valid number greater than or equal to ${Purchased}`);
                    return;
                }

                registryItem.Quantity = newQuantity;

                const { success, data } = await Post(`${_deleteUrl}`, registryItem);

                if (!success) {
                    DisplayNotification("Unable to Save Changes");
                    return;
                }

                DisplayNotification("Changes Save");
            };
        },
        onReport_Click: async () => {
            const
                { params, closeParamDialog } = await getReportParams(),
                { success, data, error } = await GetFile(_reportUrl, params);

            if (!success) {
                alert('Unable to Download Report');
                console.dir(error);
                return;
            }

            closeParamDialog();
            SaveAsFile(data, params.name);
        }
    }

export { init }