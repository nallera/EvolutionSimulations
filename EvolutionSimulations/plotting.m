clear
close all

file = fopen("C:\Users\Nico\source\repos\EvolutionSimulations\EvolutionSimulations\bin\Debug\netcoreapp3.1\logs\population.json",'r');;
fileOutput = textscan(file,'%s');
stringData = string(fileOutput{:});
fclose(file);
x = jsondecode(stringData);

friendly = x.results(:,:,1);
hostile = x.results(:,:,2);

figure
plot(friendly)
hold on
plot(hostile)
legend('Friendly','Hostile')
xlabel('day')
ylabel('number of creatures')