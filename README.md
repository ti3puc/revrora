# Tales of Revrora (nome provisório)

### Project Settings

- Unity version: **2022.3.42f1**. Download here https://unity.com/releases/editor/archive
- Render pipeline: **URP**
- Packages:
  - Hierarchy Decorator: https://github.com/WooshiiDev/HierarchyDecorator
  - Naughty Attributes: https://github.com/dbrizov/NaughtyAttributes
  - Custom Toolbar: https://github.com/smkplus/CustomToolbar

### Git Flow

- **Nomenclatura das Branches**:
  - **main**: Branch de produção. Deve conter apenas versões estáveis do jogo.
  - **develop**: Branch de desenvolvimento. Deve conter a versão mais atualizada do jogo.
  - **feature/feature-name**: Branch de desenvolvimento de uma feature específica.
  - **bugfix/bug-name**: Branch de correção de bugs.


- **Commits**:
    - **feat**: Nova feature.
    - **fix**: Correção de bugs.
    - **refactor**: Refatoração de código.
    - **style**: Alterações que não afetam o código (formatação, comentários, etc).
    - **docs**: Alterações na documentação.
    - **test**: Adição de testes.
    - **chore**: Atualizações de tarefas de build, configurações, etc.


- **Flow Pull Requests**:
    - **feature/bugfix -> develop**: Pull request de uma feature ou correção de bug para a branch de desenvolvimento.
    - **develop -> main**: Pull request da branch de desenvolvimento para a branch de produção.