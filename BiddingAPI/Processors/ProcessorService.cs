using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace BiddingAPI.Processors;

public class ProcessorService {
    private readonly ILogger<ProcessorService> logger;
    private readonly IEnumerable<IProcessor> processors;
    public ProcessorService(ILogger<ProcessorService> logger, IEnumerable<IProcessor> processors){
        this.logger = logger;
        this.processors = processors;
    }

    public void Initialize(){
        logger.LogInformation($"initializing all processors");
        foreach(var processor in processors){
            logger.LogInformation($"initializing processor: {processor.GetType().ToString()}");
            processor.Initialize();
            logger.LogInformation($"initialized processor: {processor.GetType().ToString()}");
        }
        logger.LogInformation($"initialized all processors");
    }
}
