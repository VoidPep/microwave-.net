>  This is a challenge by [Coodesh](https://coodesh.com/)

## Microwave.NET

Simulação de um microondas via web, construída em .NET 10 com arquitetura separada em API REST + frontend Razor Pages, comunicação em tempo real via SignalR.

## Estrutura de Projetos

- `Microwave.NET` -> Frontend — Razor Pages + JavaScript
- `Microwave.NET.API` -> Backend — Controllers REST + configuração de serviços |
- `Microwave.NET.Services` -> Lógica de negócio — Manager, Service, Presets |
- `Microwave.NET.DataStructures` -> DTOs, constantes, enums e definição do Hub SignalR |
- `Microwave.NET.Test` -> Testes unitários com xUnit e Moq |

Separei tudo com um formato de Layered Architecture (Arquitetura em camadas). Na verdade eu estava mirando mesmo em um Clean Architecture mas enxergo que essa separação de solution é possívelmente a mais adequada

## Design Patterns

### Singleton (MicrowaveManager)

O `MicrowaveManager` é registrado como `Singleton` no container de DI. O microondas é um recurso físico único, com estado compartilhado entre todas as requisições timer, potência, progresso, se está rodando ou pausado.
Obs.: Para o MicrowaveManager eu acho que também poderia fazer algum controlador de estado e usar um State design pattern (https://refactoring.guru/design-patterns/state) 
Porém achei mais fácil registrar um singleton mesmo

### Strategy (Presets de programa)

Cada preset de alimento (`PresetPipoca`, `PresetFrango`, etc.) implementa a interface `IPresetProgram` e herda de `BasePresetedProgram`. Isso permite registrar todos no container como `IEnumerable<IPresetProgram>` e resolver o correto em runtime pelo nome, sem nenhum `if/switch`. Adicionar um novo preset é só criar uma classe nova e registrá-la no DI.
Deixa bem mais limpo, fica um pouco mais pesado, mas da uma manutenção bem maior depois ;)

### Template Method ou quase isso (BasePresetedProgram)

`BasePresetedProgram` é um template method mais aplicado a dados do que aos comportamento. Cada subclasse concreta preenche os valores abstratos.
Porém se tiver um caso futuro em que cada um dos presets executa algum comportamento, dae sim essa classe passa a ser um template method por completo

## Por que SignalR?

Ta, eu poderia fazer um polling no front-end, só que isso deixa muito pesado a tela, dar um setTimeout($.ajax(), 1000) seria custoso pra caramba, a ideia do signalR é justamente dar uma escalabilidade e desacoplamento

## Relação de MicrowaveManager e MicrowaveService

Um é um controlador e o outro é um "Mantainer de estado", como eu preciso que o tempo do temporizador seja algo mais flexivo, eu consigo alterar as informações diretamente no singleton, enquanto o loop do `StartHeatingAsync()`
fica rodando em background

Falando em background inclusive, acredito que pra mais escalabilidade ainda, muito provávelmente esse serviço em background poderia ficar em uma scheduled class usando `Hangfire.NET`, porém por agora isso seria matar um mosquito com uma bazooka

