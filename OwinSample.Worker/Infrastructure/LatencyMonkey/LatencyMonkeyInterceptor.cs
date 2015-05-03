using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using Nimbus.Interceptors.Inbound;
using Nimbus.MessageContracts;
using OwinSample.Worker.ConfigurationSettings;
using OwinSample.Worker.Infrastructure.Environments;
using Serilog;

namespace OwinSample.Worker.Infrastructure.LatencyMonkey
{
    public class LatencyMonkeyInterceptor : InboundInterceptor
    {
        private static bool _alreadyLogged;
        private readonly bool _enabled;

        public LatencyMonkeyInterceptor(LatencyMonkeyEnabled latencyMonkeyEnabled)
        {
            _enabled = latencyMonkeyEnabled.Value && AppEnvironment.IsLocal();

            if (!_enabled) return;

            // Only log the fact Latency Monkey is enabled once
            if (_alreadyLogged) return;
            _alreadyLogged = true;

            const string latencyMonkey = @"LATENCY MONKEY ENABLED
                               __,__
                     .--.  .-""     ""-.  .--.
                    / .. \/  .-. .-.  \/ .. \
                   | |  '|  /   Y   \  |'  | |
                   | \   \  \ 0 | 0 /  /   / |
                    \ '- ,\.-""`` ``""-./, -' /
                     `'-' /_   ^ ^   _\ '-'`
                     .--'|  \._ _ _./  |'--.
                   /`    \   \.-.  /   /    `\
                  /       '._/  |-' _.'       \
                 /          ;  /--~'   |       \
                /        .'\|.-\--.     \       \
               /   .'-. /.-.;\  |\|'~'-.|\       \
               \       `-./`|_\_/ `     `\'.      \
                '.      ;     ___)        '.`;    /
                  '-.,_ ;     ___)          \/   /
                   \   ``'------'\       \   `  /
                    '.    \       '.      |   ;/_
                  ___>     '.       \_ _ _/   ,  '--.
                .'   '.   .-~~~~~-. /     |--'`~~-.  \
               // / .---'/  .-~~-._/ / / /---..__.'  /
              ((_(_/    /  /      (_(_(_(---.__    .'
                        | |     _              `~~`
                        | |     \'.
                         \ '....' |
                          '.,___.'";

            Log.Warning(latencyMonkey);
        }

        public override async Task OnCommandHandlerExecuting<TBusCommand>(TBusCommand busCommand,
            BrokeredMessage brokeredMessage)
        {
            await LatencyMonkey(busCommand.GetType());
            await base.OnCommandHandlerExecuting(busCommand, brokeredMessage);
        }

        public override async Task OnMulticastRequestHandlerExecuting<TBusRequest, TBusResponse>(
            IBusMulticastRequest<TBusRequest, TBusResponse> busRequest, BrokeredMessage brokeredMessage)
        {
            await LatencyMonkey(busRequest.GetType());
            await base.OnMulticastRequestHandlerExecuting(busRequest, brokeredMessage);
        }

        public override async Task OnRequestHandlerExecuting<TBusRequest, TBusResponse>(
            IBusRequest<TBusRequest, TBusResponse> busRequest, BrokeredMessage brokeredMessage)
        {
            await LatencyMonkey(busRequest.GetType());
            await base.OnRequestHandlerExecuting(busRequest, brokeredMessage);
        }

        public override async Task OnEventHandlerExecuting<TBusEvent>(TBusEvent busEvent,
            BrokeredMessage brokeredMessage)
        {
            await LatencyMonkey(busEvent.GetType());
            await base.OnEventHandlerExecuting(busEvent, brokeredMessage);
        }

        private async Task LatencyMonkey(Type messageType)
        {
            var filter = new List<Type>();

            if (!_enabled || filter.Contains(messageType))
                return;

            var random = new Random((int) DateTime.Now.Ticks);
            var delay = random.Next(0, 1000);

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Latency monkey has decided to make you wait {0}ms", delay);
            Console.ResetColor();

            await Task.Delay(delay);
        }
    }
}