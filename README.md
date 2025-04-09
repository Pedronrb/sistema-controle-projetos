Sistema de Controle de Projetos – Dell Premier (Nivelamento)

Este projeto integra o nivelamento prático do programa Dell Premier, com o objetivo de aplicar conhecimentos em .NET (C#) no desenvolvimento de um sistema completo para gerenciamento de projetos acadêmicos.
Objetivo do Projeto

O sistema tem como finalidade permitir o cadastro de usuários (alunos e professores), o gerenciamento de projetos acadêmicos e a vinculação de alunos com funções específicas dentro desses projetos. Também contempla autenticação segura utilizando JWT e testes automatizados.
Funcionalidades
Autenticação e Usuários

    Login com token JWT.

    Cadastro de usuários com distinção entre aluno e professor.

        Professores possuem campos adicionais: área de atuação e formação.

Gestão de Projetos

    Cadastro de projetos (restrito a professores).

    O professor que cria o projeto é automaticamente definido como coordenador.

    Vinculação de alunos a projetos com definição de funções específicas:

        Estagiário, Júnior, Sênior e Master.

    Listagem e detalhamento de projetos:

        Nome, descrição, coordenador responsável e integrantes com suas respectivas funções.

Testes

    Implementação de testes unitários e de integração para assegurar o correto funcionamento do sistema.

Tecnologias Utilizadas
Backend (.NET)

    C# / ASP.NET Core

    Entity Framework Core

    LocalDb

    Autenticação com JWT

    Testes com xUnit

