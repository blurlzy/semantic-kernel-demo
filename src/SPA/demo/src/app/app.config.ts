import { ApplicationConfig, provideZoneChangeDetection, importProvidersFrom, ErrorHandler } from '@angular/core';
import { provideRouter } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
// http module
import { provideHttpClient, withInterceptorsFromDi, HTTP_INTERCEPTORS, withFetch, } from '@angular/common/http';
import { provideNoopAnimations } from '@angular/platform-browser/animations';
// msal module
import { IPublicClientApplication, PublicClientApplication, InteractionType, BrowserCacheLocation, LogLevel, } from '@azure/msal-browser';
import {
  MsalInterceptor, MSAL_INSTANCE, MsalInterceptorConfiguration, MsalGuardConfiguration,
  MSAL_GUARD_CONFIG, MSAL_INTERCEPTOR_CONFIG, MsalService, MsalGuard, MsalBroadcastService,
} from '@azure/msal-angular';

// markdown module
import { MarkdownModule, CLIPBOARD_OPTIONS, ClipboardButtonComponent } from 'ngx-markdown';

// app routes
import { routes } from './app.routes';
// env
import { environment } from '../environments/environment';
// services
import { GlobalErrorHandler } from './core/error-handler.service';

// app config
export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    importProvidersFrom(
      BrowserModule,
      MarkdownModule.forRoot({
        clipboardOptions: {
          provide: CLIPBOARD_OPTIONS,
          useValue: {
            buttonComponent: ClipboardButtonComponent,
          },
        },
      }),
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
    // error handler    
    { provide: ErrorHandler,  useClass: GlobalErrorHandler}
  ],
};

// msal config
export function loggerCallback(logLevel: LogLevel, message: string) {
  console.log(message);
}

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
    // system: {
    //   allowNativeBroker: false, // Disables WAM Broker
    //   loggerOptions: {
    //     loggerCallback,
    //     logLevel: LogLevel.Info,
    //     piiLoggingEnabled: false,
    //   },
    // },
  });
}

export function MSALInterceptorConfigFactory(): MsalInterceptorConfiguration {
  const protectedResourceMap = new Map<string, Array<string>>();
  // copilot api
  protectedResourceMap.set(
    environment.copilotApiConfig.uri,
    environment.copilotApiConfig.scopes
  );
  // MS Graph
  protectedResourceMap.set(
    environment.apiConfig.uri,
    environment.apiConfig.scopes
  );

  return {
    interactionType: InteractionType.Redirect,
    protectedResourceMap,
  };
}

export function MSALGuardConfigFactory(): MsalGuardConfiguration {
  return {
    interactionType: InteractionType.Redirect,
    // authRequest: {
    //   scopes: [...environment.apiConfig.scopes],
    // },
    loginFailedRoute: '/login-failed',
  };
}