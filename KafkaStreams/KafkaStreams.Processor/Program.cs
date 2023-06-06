IConfiguration configuration = Extensions.GetConfiguration();
IServiceProvider services = Extensions.GetServices(configuration);

var streamConfig = Extensions.GetStreamConfig(configuration);
var topology = TopologyBuilder.Build();

var stream = new KafkaStream(topology, streamConfig);
await stream.StartAsync();

Console.WriteLine("Processor started. To stop the processor press Ctrl+C.");