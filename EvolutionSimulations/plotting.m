clear
close all

file = fopen("C:\Users\Nico\source\repos\EvolutionSimulations\EvolutionSimulations\bin\Debug\netcoreapp3.1\logs\population.json",'r');;
fileOutput = textscan(file,'%s');
stringData = string(fileOutput{:});
fclose(file);
x = jsondecode(stringData);

friendly = zeros(size(x,1),50);
hostile = zeros(size(x,1),50);

for i = 1:size(x,1)
    
    friendly(i,1:length(x(i).results(:,:,1))) = x(i).results(:,:,1);
    hostile(i,1:length(x(i).results(:,:,2))) = x(i).results(:,:,2);
    
end

friendlyMean = mean(friendly,1);
hostileMean = mean(hostile,1);

figure
hold on
plot(friendlyMean)
plot(hostileMean)
legend('Friendly','Hostile')
xlabel('day')
ylabel('number of creatures')