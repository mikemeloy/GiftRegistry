import { Get } from '../../modules/utils.js';

let
    _registryRoute,
    _consultantRoute,
    _registryTypeRoute;

const
    init = (registryRoute, consultantRoute, registryTypeRoute) => {
        _consultantRoute = consultantRoute;
        _registryRoute = registryRoute;
        _registryTypeRoute = registryTypeRoute;

        setupFormEvents();
    },
    setupFormEvents = () => {
        const [registry, consultants, registryType] = document.querySelectorAll('[data-registry-admin-header] nav button');

        registry.addEventListener('click', events.onRegistry_Click);
        consultants.addEventListener('click', events.onConsultant_Click);
        registryType.addEventListener('click', events.onRegistryType_Click);
    },
    appendPartial = (partial) => {
        const
            main = document.querySelector('main[data-registry-admin-main]'),
            options = { duration: 300, fill: "forwards" };
        return new Promise((res) => {
            main.animate({ opacity: [1, 0] }, options)
                .addEventListener('finish', () => {
                    main.innerHTML = partial;

                    main.animate({ opacity: [0, 1] }, options)
                        .addEventListener('finish', () => {
                            res(main);
                        });
                });
        });

    };

const events = {
    onRegistry_Click: async () => {
        const { data } = await Get(`${_registryRoute}/List`),
            el = await appendPartial(data),
            { init } = await import('./Tabs/registry.js');

        init?.(el, _registryRoute);
    },
    onConsultant_Click: async () => {
        const { data } = await Get(`${_consultantRoute}/List`),
            el = await appendPartial(data),
            { init } = await import('./Tabs/registryConsultant.js');

        init?.(el, _consultantRoute);
    },
    onRegistryType_Click: async () => {
        const { data } = await Get(`${_registryTypeRoute}/List`),
            el = await appendPartial(data),
            { init } = await import('./Tabs/registryType.js');

        init?.(el, _registryTypeRoute);
    }
}

export { init }