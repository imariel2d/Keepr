# Changelog

## [1.3.0](https://github.com/imariel2d/keepr/compare/v1.2.0...v1.3.0) (2026-08-10)


### Features

* **api:** localize outbound emails per recipient ([#30](https://github.com/imariel2d/keepr/issues/30) Phase 3) ([f127b29](https://github.com/imariel2d/keepr/commit/f127b2933ddf67974accbbd3d96bdd5f704a75a6))
* **api:** preferred language on account ([#30](https://github.com/imariel2d/keepr/issues/30)) ([945bb98](https://github.com/imariel2d/keepr/commit/945bb98adda671d22742b478d67c44902b2640c3))
* **api:** stable error codes on every user-facing problem ([#30](https://github.com/imariel2d/keepr/issues/30)) ([95f7584](https://github.com/imariel2d/keepr/commit/95f7584f0dbaee2fdb7df25a1b63f741b18896ed))
* **client:** i18n foundation + localized login + locale serving ([#30](https://github.com/imariel2d/keepr/issues/30)) ([6b23c2b](https://github.com/imariel2d/keepr/commit/6b23c2b2a7109cfa8c49dd240ba9d41616ea2a71))
* **client:** localize the admin screens ([#30](https://github.com/imariel2d/keepr/issues/30)) ([5871d37](https://github.com/imariel2d/keepr/commit/5871d37f9dd1e4f72fb7a2804bd38504ae1beaca))
* **client:** localize the app shell/chrome ([#30](https://github.com/imariel2d/keepr/issues/30)) ([d4cf5e8](https://github.com/imariel2d/keepr/commit/d4cf5e86330063ea4e94d0581978fb29d378719e))
* **client:** localize the auth/entry screens ([#30](https://github.com/imariel2d/keepr/issues/30)) ([cdad576](https://github.com/imariel2d/keepr/commit/cdad576ea4b0917ba75e42e25df6f597c427bf03))
* **client:** localize the Files feature ([#30](https://github.com/imariel2d/keepr/issues/30)) ([943be0c](https://github.com/imariel2d/keepr/commit/943be0ccf633fd861b865df5cf355c74f2e7365d))
* **client:** localize the public share viewer ([#30](https://github.com/imariel2d/keepr/issues/30)) ([d9ce4fb](https://github.com/imariel2d/keepr/commit/d9ce4fb061a90e3876c79de57610d19417955e59))
* **client:** localize the Trash screen ([#30](https://github.com/imariel2d/keepr/issues/30)) ([72b9e40](https://github.com/imariel2d/keepr/commit/72b9e4008577a6819d18b8795198514e72b520ef))
* localize field-validation errors via codes ([#30](https://github.com/imariel2d/keepr/issues/30) Phase 2) ([bc743f2](https://github.com/imariel2d/keepr/commit/bc743f2c2e67a15596c3de6445d5731857cbe806))


### Bug Fixes

* **api:** redirect unprefixed paths to the locale build; keep e2e green ([#30](https://github.com/imariel2d/keepr/issues/30)) ([f4c3623](https://github.com/imariel2d/keepr/commit/f4c3623ae775c81a7c9d4f06f1a04c3636a5cb56))

## [1.2.0](https://github.com/imariel2d/keepr/compare/v1.1.0...v1.2.0) (2026-08-07)


### Features

* **api:** change email backend ([#27](https://github.com/imariel2d/keepr/issues/27)) ([a68f95a](https://github.com/imariel2d/keepr/commit/a68f95a12e4b6043d512570c3f9e51002f539a2a))
* change email ([#27](https://github.com/imariel2d/keepr/issues/27)) — design + backend + UI ([f438217](https://github.com/imariel2d/keepr/commit/f438217fadb14af59c782423e1f161670127b991))
* **client:** change email UI ([#27](https://github.com/imariel2d/keepr/issues/27)) ([a85ba1e](https://github.com/imariel2d/keepr/commit/a85ba1efa1f8d172bb725aea3c1624bc5077820d))


### Bug Fixes

* **api:** address CodeRabbit review on [#27](https://github.com/imariel2d/keepr/issues/27) change email ([131981c](https://github.com/imariel2d/keepr/commit/131981c4fdfb02254092700787f6985b11d326d8))

## [1.1.0](https://github.com/imariel2d/keepr/compare/v1.0.0...v1.1.0) (2026-08-03)


### Features

* **api:** password reset backend ([#26](https://github.com/imariel2d/keepr/issues/26)) ([e5487ec](https://github.com/imariel2d/keepr/commit/e5487ec7ff160534a06e6a597e89c2f0c684a52f))
* **api:** password reset backend ([#26](https://github.com/imariel2d/keepr/issues/26)) ([eec42ea](https://github.com/imariel2d/keepr/commit/eec42eafecbd062b382a27b8d8da94ceb2f37774))
* **client:** password reset UI ([#26](https://github.com/imariel2d/keepr/issues/26)) ([d0c029c](https://github.com/imariel2d/keepr/commit/d0c029cc4d0b1359bd7867b6025135eeb34e75c9))
* collapse admin row actions into a three-dot menu ([c9e1ecb](https://github.com/imariel2d/keepr/commit/c9e1ecb825bff687eb9e8e8b59b0b3648cd9acf4))
* password reset ([#26](https://github.com/imariel2d/keepr/issues/26)) — design + backend ([45426db](https://github.com/imariel2d/keepr/commit/45426db4931b9e3f491831e6b26724f9aa17cbf2))


### Bug Fixes

* **api:** address CodeRabbit review on [#26](https://github.com/imariel2d/keepr/issues/26) password reset ([655fa98](https://github.com/imariel2d/keepr/commit/655fa987a58db6cd69e318cb07dec1c4825dcdbe))

## 1.0.0 (2026-08-03)


### Features

* **#36:** admin email-settings screen (/admin/email) ([051a713](https://github.com/imariel2d/keepr/commit/051a713ec1aeec98f86c83afc4f2351de73c00dd))
* **#36:** implement admin-managed email providers (backend) ([42e15d1](https://github.com/imariel2d/keepr/commit/42e15d152dd50fe3c6281635e7e9a89e2caccc41))
* **#36:** move Email into an expandable Admin sidebar group ([0c69563](https://github.com/imariel2d/keepr/commit/0c69563edf56f3b6d771840ea95166c7a624d23b))
* **email:** show inviter name in invites, surface active provider, add e2e-design skill ([05b1c9c](https://github.com/imariel2d/keepr/commit/05b1c9cf7e65d26814ed542433feed197eedc0f2))


### Bug Fixes

* **#36:** address CodeRabbit review on the admin-nav pass ([fb33508](https://github.com/imariel2d/keepr/commit/fb335087d48b3abbc2191729d2e447afa95b0c10))
* **#36:** address CodeRabbit review on the email-providers backend ([a71075b](https://github.com/imariel2d/keepr/commit/a71075ba6c3c260c2ee33f3a19427ee8e7dce495))
