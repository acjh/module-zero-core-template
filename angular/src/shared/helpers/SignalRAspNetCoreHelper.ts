import { AppConsts } from '@shared/AppConsts';
import { UtilsService } from '@abp/utils/utils.service';

export class SignalRAspNetCoreHelper {
    static initSignalR(): void {

        var encryptedAuthToken = new UtilsService().getCookieValue(AppConsts.authorization.encrptedAuthTokenName);

        abp.signalr = {
            autoConnect: true,
            connect: undefined,
            hubs: undefined,
            qs: AppConsts.authorization.encrptedAuthTokenName + "=" + encodeURIComponent(encryptedAuthToken),
            url: AppConsts.remoteServiceBaseUrl + '/signalr'
        };

        jQuery.getScript(AppConsts.appBaseUrl + '/assets/abp/abp.signalr-client.js', () => {
            abp.signalr.hubs.common.on('getMessage', (data: any) => {
                console.log(data);
            });
        });
    }
}
