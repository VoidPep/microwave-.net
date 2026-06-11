using System;
using System.Collections.Generic;
using System.Text;

namespace Microwave.NET.Services.Interfaces;

/// <summary>
/// A ideia é fazer com que esse serviço seja responsável por gerenciar o estado do microondas em background, para não precisar travar a requisição
/// </summary>
public interface IMicrowaveManager
{
    void Start(string key, out CancellationTokenSource ct);
    void Stop(string key);
}
