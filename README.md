# 🧩 Sistema de Controle de Projetos – Dell Premier (Nivelamento)

Este projeto faz parte de um **nivelamento prático** para o programa **Dell Premier**, com o objetivo de aplicar conhecimentos em **.NET (C#)** e **Angular** no desenvolvimento de um sistema completo de gerenciamento de projetos acadêmicos.

---

## 📌 Objetivo do Projeto

O sistema permite o **cadastro de usuários (alunos e professores)**, gerenciamento de **projetos acadêmicos**, e a **vinculação de alunos com funções específicas** dentro desses projetos. Além disso, inclui **autenticação segura com JWT** e **testes automatizados**.

---

## 🚀 Funcionalidades

### 🛠️ Autenticação e Usuários
- [x] Login com token JWT
- [x] Cadastro de usuário com distinção entre aluno e professor
  - Professores possuem campos adicionais: **área de atuação** e **formação**

### 📂 Gestão de Projetos
- [x] Cadastro de projetos (somente professores)
- [x] Professor que cria o projeto torna-se automaticamente **coordenador**
- [x] Vinculação de alunos a projetos com funções específicas:
  - Estagiário, Júnior, Sênior, Master
- [x] Listagem e detalhamento dos projetos:
  - Nome, descrição, coordenador e equipe com respectivas funções

### 🧪 Testes
- [x] Testes unitários e de integração para garantir a funcionalidade da aplicação

---

## 🛠️ Tecnologias Utilizadas

### Backend (.NET)
- C# / ASP.NET Core
- Entity Framework Core
- SQL Server / SQLite
- Autenticação JWT
- Testes com xUnit ou MST
