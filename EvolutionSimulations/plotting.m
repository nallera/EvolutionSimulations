clear
close all

%% Parameters

file = fopen("C:\Users\Nico\source\repos\EvolutionSimulations\EvolutionSimulations\bin\Debug\netcoreapp3.1\logs\parameters.json",'r');
fileOutput = textscan(file,'%s');
fclose(file);
stringData = string(fileOutput{:});
parameters = jsondecode(stringData);

xLimit = parameters.xLimit;
yLimit = parameters.yLimit;
simulationDays = parameters.simulationDays;
stepsPerDay = parameters.stepsPerDay;
foodPerDay = parameters.foodPerDay;
foodToSurvive = parameters.foodToSurvive;
foodToReproduce = parameters.foodToReproduce;
numberOfSimulations = parameters.numberOfSimulations;
logOnlyPopulation = parameters.logOnlyPopulation;
CreatureTypes = parameters.CreatureTypes;
numberOfCreatureTypes = size(CreatureTypes,1);

%% Population

file = fopen("C:\Users\Nico\source\repos\EvolutionSimulations\EvolutionSimulations\bin\Debug\netcoreapp3.1\logs\population.json",'r');
fileOutput = textscan(file,'%s');
fclose(file);
stringData = string(fileOutput{:});
populationData = jsondecode(stringData);

populations = zeros(numberOfCreatureTypes,numberOfSimulations,simulationDays);

for simulationIndex = 1:numberOfSimulations
    
    for creatureType = 1:numberOfCreatureTypes
        populations(creatureType,simulationIndex,1:length(populationData(simulationIndex).results(:,:,creatureType))) = ...
        populationData(simulationIndex).results(:,:,creatureType);
    end
end

populationsMean = reshape(mean(populations,2),numberOfCreatureTypes,simulationDays);
populationsMean(numberOfCreatureTypes + 1,:) = sum(populationsMean,1);

%% Plotting

creatureTypeColor = rand(numberOfCreatureTypes,1,3);

figure
title('Population mean')
hold on
for creatureType = 1:numberOfCreatureTypes
    plot(populationsMean(creatureType,:),'Color',creatureTypeColor(creatureType,:,:),...
        'DisplayName',CreatureTypes{creatureType});
end
plot(populationsMean(numberOfCreatureTypes + 1,:),'-k',...
    'DisplayName','Total');
xlabel('Day')
ylabel('Number of creatures')
legend('Location','best')

figure
title('Populations')
hold on
for creatureType = 1:numberOfCreatureTypes
    for simulationIndex = 1:numberOfSimulations
        plot(reshape(populations(creatureType,simulationIndex,:),1,simulationDays),...
            'Color',creatureTypeColor(creatureType,:,:),'HandleVisibility','off');
    end
    plot(0,0,'Color',creatureTypeColor(creatureType,:,:),'DisplayName',CreatureTypes{creatureType})
end
xlabel('Day')
ylabel('Number of creatures')
legend('Location','best')
