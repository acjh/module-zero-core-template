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
            remoteServiceBaseUrl: AppConsts.remoteServiceBaseUrl,
            startConnection: undefined,
            url: '/signalr'
        };

        jQuery.getScript(AppConsts.appBaseUrl + '/assets/abp/abp.signalr-client.js', () => {
            abp.signalr.hubs.common.on('getMessage', (data: any) => {
                console.log(data);
            });

            var chatHub;

            abp.signalr.startConnection('/signalr-myChatHub', function (connection) {
                chatHub = connection; // Save a reference to the hub

                connection.on('getMessage', function (message) { // Register for incoming messages
                    console.log('received message: ' + message);
                });
            }).then(function (connection) {
                abp.log.debug('Connected to myChatHub server!');
                abp.event.trigger('myChatHub.connected');
            });

            abp.event.on('myChatHub.connected', function() { // Register for connect event
                chatHub.invoke('sendMessage', "Hi everybody, I'm connected to the chat!"); // Send a message to the server
            });
        });
    }
}
