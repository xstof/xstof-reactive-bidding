using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace BiddingAPI.Processors;

public static class ProcessorAppBuilderExtensions{
    public static void UseProcessors(this IApplicationBuilder builder){
        var processorService = (ProcessorService) builder.ApplicationServices.GetRequiredService(typeof(ProcessorService));
        processorService.Initialize();
    }
}
