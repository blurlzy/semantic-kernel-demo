# demo app

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 18.2.3.

## Development server

Run `npm install` to install the dependencies.
Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The application will automatically reload if you change any of the source files.

## Microsoft Authentication Library for Angular

```
https://github.com/AzureAD/microsoft-authentication-library-for-js
```

## Prerequisites

Before using `@azure/msal-angular`, [register an application in Azure AD](https://docs.microsoft.com/azure/active-directory/develop/quickstart-register-app) to get your `clientId`.

## Installation

The `@azure/msal-angular` package is available on NPM:

```
npm install @azure/msal-browser @azure/msal-angular@latest
```

### Configure MSAL moduele
```
    msalConfig: {
      auth: {
        clientId: '<app-registration-client-id>',
        authority: 'https://login.microsoftonline.com/<azure-tenant-id>',
      },
    },
    copilotApiConfig: {
      scopes: ['api://<app-registration-api-client-id>/<scope>'],
      uri: '<copilot-api-endpoint>',
    }
```

```
export const appConfig: ApplicationConfig = {
	providers: [
		provideZoneChangeDetection({ eventCoalescing: true }),
		provideRouter(routes),
		importProvidersFrom(
			BrowserModule,
			MarkdownModule.forRoot(),
		),
		provideNoopAnimations(),
		// register msal interceptor
		provideHttpClient(withInterceptorsFromDi(), withFetch()),
		{
			provide: HTTP_INTERCEPTORS,
			useClass: MsalInterceptor,
			multi: true,
		},
		{
			provide: MSAL_INSTANCE,
			useFactory: MSALInstanceFactory,
		},
		{
			provide: MSAL_GUARD_CONFIG,
			useFactory: MSALGuardConfigFactory,
		},
		{
			provide: MSAL_INTERCEPTOR_CONFIG,
			useFactory: MSALInterceptorConfigFactory,
		},
		MsalService,
		MsalGuard,
		MsalBroadcastService, provideAnimationsAsync(),

	],
};
```

```
export function MSALInstanceFactory(): IPublicClientApplication {
	return new PublicClientApplication({
		auth: {
			clientId: environment.msalConfig.auth.clientId,
			authority: environment.msalConfig.auth.authority,
			redirectUri: '/',
			postLogoutRedirectUri: '/',
		},
		cache: {
			cacheLocation: BrowserCacheLocation.LocalStorage,
		},
		system: {
			allowNativeBroker: false, // Disables WAM Broker
			loggerOptions: {
			loggerCallback,
			logLevel: LogLevel.Info,
			piiLoggingEnabled: false,
			},
		},
	});
}

export function MSALInterceptorConfigFactory(): MsalInterceptorConfiguration {
	const protectedResourceMap = new Map<string, Array<string>>();
	// copilot api
	protectedResourceMap.set(
		environment.copilotApiConfig.uri,
		environment.copilotApiConfig.scopes
	);

	return {
		interactionType: InteractionType.Redirect,
		protectedResourceMap,
	};
}

export function MSALGuardConfigFactory(): MsalGuardConfiguration {
	return {
		interactionType: InteractionType.Redirect,
		loginFailedRoute: '/login-failed',
	};
}
	
export function loggerCallback(logLevel: LogLevel, message: string) {
	console.log(message);
}

```

### Using the MSAL Guard to protect routes and require authentication before accessing the protected route.
```
export const public_routes: Routes = [
	{ path: '', component: ChatComponent, canActivate: [MsalGuard] },
];
```