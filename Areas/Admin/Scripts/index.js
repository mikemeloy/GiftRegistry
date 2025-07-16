import { Get, AddQueryParamToURL, GetQueryParam, FadeOut } from '../../modules/utils.js';

let
    _registryRoute,
    _consultantRoute,
    _registryTypeRoute,
    _registryShippingRoute,
    _deleteItemRoute,
    _reportGenerationRoute;

const
    init = (registryRoute, consultantRoute, registryTypeRoute, registryShippingRoute, deleteItemRoute, reportGenerationRoute) => {
        _consultantRoute = consultantRoute;
        _registryRoute = registryRoute;
        _registryTypeRoute = registryTypeRoute;
        _registryShippingRoute = registryShippingRoute;
        _deleteItemRoute = deleteItemRoute;
        _reportGenerationRoute = reportGenerationRoute;

        setupFormEvents();
        initTab();
    },
    setupFormEvents = () => {
        const
            [
                registry, consultants,
                registryType, shipping
            ] = document.querySelectorAll('[data-registry-admin-header] nav button'),
            container = document.querySelector('main[data-registry-admin-main]');

        registry.addEventListener('click', events.onRegistry_Click);
        consultants.addEventListener('click', events.onConsultant_Click);
        registryType.addEventListener('click', events.onRegistryType_Click);
        shipping.addEventListener('click', events.onShippingType_Click);
        container.addEventListener('refresh', events.onRefresh_Click);
    },
    appendPartial = async (partial) => {
        const
            main = document.querySelector('main[data-registry-admin-main]'),
            onFadeComplete = await FadeOut(main);

        main.innerHTML = partial;

        await onFadeComplete();

        return main;
    },
    setTabQuery = (value) => {
        AddQueryParamToURL([{ key: 'tab', value }]);
    },
    initTab = () => {
        const param = GetQueryParam('tab') ?? 'home';
        events.onRefresh_Click({ detail: param })
    };

const events = {
    onRegistry_Click: async () => {
        const { data } = await Get(`${_registryRoute}/List`),
            el = await appendPartial(data),
            { init } = await import('./Tabs/registry.js');

        init?.(el, _registryRoute, _deleteItemRoute, _reportGenerationRoute);
        setTabQuery('home');
    },
    onConsultant_Click: async () => {
        const { data } = await Get(`${_consultantRoute}/List`),
            el = await appendPartial(data),
            { init } = await import('./Tabs/registryConsultant.js');

        init?.(el, _consultantRoute);
        setTabQuery('consultant');
    },
    onRegistryType_Click: async () => {
        const { data } = await Get(`${_registryTypeRoute}/List`),
            el = await appendPartial(data),
            { init } = await import('./Tabs/registryType.js');

        init?.(el, _registryTypeRoute);
        setTabQuery('type');
    },
    onShippingType_Click: async () => {
        const { data } = await Get(`${_registryShippingRoute}/List`),
            el = await appendPartial(data),
            { init } = await import('./Tabs/registryShippingOption.js');

        init?.(el, _registryShippingRoute);
        setTabQuery('ship');
    },
    onRefresh_Click: async ({ detail = 'home' }) => {
        var fn = {
            home: events.onRegistry_Click,
            ship: events.onShippingType_Click,
            consultant: events.onConsultant_Click,
            type: events.onRegistryType_Click
        }[detail];

        fn?.();
    }
}

export { init }