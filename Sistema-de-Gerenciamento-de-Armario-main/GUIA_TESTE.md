# 🧪 GUIA DE TESTE - Verificar as Correções

## ✅ CHECKLIST DE TESTE

### Teste 1: Inicialização do Banco de Dados
**Objetivo:** Verificar se o BD é criado e inicializado automaticamente

**Passos:**
1. Delete o arquivo `setor_armarios.sqlite` se existir
2. Execute a aplicação (`F5`)
3. ✅ **Esperado:** Nenhuma exceção, app abre normalmente
4. ✅ **Verificar:** Arquivo `setor_armarios.sqlite` foi criado no diretório do programa

**Se falhar:**
- ❌ Erro: "Erro ao inicializar aplicação"
- **Solução:** Verificar console do Visual Studio para mais detalhes

---

### Teste 2: Dados Padrão Inseridos
**Objetivo:** Verificar se vestiários e perfis foram inseridos

**Passos:**
1. Execute a aplicação
2. Vá para a tela de cadastro
3. ✅ **Esperado:** 
   - Opção "Feminino" aparece no radio button
   - Opção "Masculino" aparece no radio button
4. Selecione "Feminino"
5. ✅ **Esperado:** 
   - 10 botões de armários aparecem
   - Cores: Verde (disponível) e Vermelho (ocupado)

**Se falhar:**
- ❌ Sem radio buttons ou vazios
- **Solução:** Verificar se `DbInitializer.Inicializar()` foi chamado

---

### Teste 3: Carregamento de Armários
**Objetivo:** Verificar se armários carregam sem erros NULL

**Passos:**
1. Na tela de cadastro, clique em "Feminino"
2. ✅ **Esperado:** Armários carregam e mostram corretamente
3. Clique em "Masculino"
4. ✅ **Esperado:** Armários trocam corretamente (sem congelar)

**Se falhar:**
- ❌ Erro: "Object reference not set to an instance"
- **Solução:** DataTable? agora protege contra null

---

### Teste 4: Seleção de Armário (Type-Safe)
**Objetivo:** Verificar se o tipo-safe pattern matching funciona

**Passos:**
1. Selecione "Feminino"
2. Clique em qualquer armário verde (disponível)
3. ✅ **Esperado:** Armário muda para azul (selecionado)
4. Clique em outro armário
5. ✅ **Esperado:** Anterior volta ao verde, novo fica azul

**Se falhar:**
- ❌ Erro de cast
- **Solução:** Novo padrão `if (sender is not Button btnClicado)` protege isso

---

### Teste 5: Cadastro de Funcionário
**Objetivo:** Verificar se funcionário é inserido corretamente

**Passos:**
1. Selecione "Feminino"
2. Selecione um armário verde
3. Digite: Nome = "João Silva"
4. Digite: Matrícula = "12345"
5. Clique "Enviar"
6. ✅ **Esperado:** 
   - Mensagem "Cadastro realizado com sucesso!"
   - Formulário limpa
   - Armário agora mostra vermelho (ocupado)

**Se falhar:**
- ❌ Erro de database
- **Solução:** Transação com rollback protege integridade

---

### Teste 6: Avisos de Compilação
**Objetivo:** Verificar se não há mais avisos

**Passos:**
1. Abra Visual Studio → Build → Rebuild Solution
2. ✅ **Esperado:** 
   ```
   Construir êxito em XXs
   (sem avisos listados)
   ```
3. Verificar Output window → Abas "Erros"
4. ✅ **Esperado:** 0 erros, 0 avisos

**Se falhar:**
- ❌ Avisos CS8603, CS8625, etc.
- **Solução:** DataTable? adiciona segurança de tipo

---

## 🔍 TESTE AVANÇADO: Verificação de Banco de Dados

### Inspecionar BD com DB Browser for SQLite

1. Download: https://sqlitebrowser.org/
2. Abra `setor_armarios.sqlite`
3. Verifique:

**Tabela: vestiario**
```
┌─────────────────────────┐
│ id │ vestiario           │
├─────────────────────────┤
│ 1  │ Feminino            │
│ 2  │ Masculino           │
└─────────────────────────┘
```
✅ **Esperado:** 2 registros

**Tabela: armario**
```
┌──────────────────────────────────┐
│ id │ numero_armario │ vestiario_id │
├──────────────────────────────────┤
│ 1  │ 1              │ 1            │
│ 2  │ 2              │ 1            │
│ ... (20 armários total) ...
└──────────────────────────────────┘
```
✅ **Esperado:** 20 armários (10 por vestiário)

**Tabela: perfil**
```
┌────────────────────────────────────────┐
│ id │ permissao              │ funcao    │
├────────────────────────────────────────┤
│ 1  │ Acesso básico         │ Funcionario     │
│ 2  │ Acesso administrativo │ Administrador   │
└────────────────────────────────────────┘
```
✅ **Esperado:** 2 perfis

---

## 📊 RESULTADOS ESPERADOS

### ✅ Compilação
- [ ] Sem erros
- [ ] Sem avisos
- [ ] Build time < 5 segundos

### ✅ Execução
- [ ] App inicia sem exceções
- [ ] BD é criado automaticamente
- [ ] Vestiários aparecem
- [ ] Armários carregam
- [ ] Cadastro funciona

### ✅ Funcionalidades
- [ ] Pattern matching funciona (type-safe)
- [ ] Nullability é respeitada
- [ ] Armários mostram status correto
- [ ] Seleção de armário é visual

---

## 🐛 TROUBLESHOOTING

| Sintoma | Causa Provável | Solução |
|---------|---|---|
| "Erro ao inicializar aplicação" | `DbInitializer.Inicializar()` falhou | Verificar Output window |
| "No such table: vestiario" | BD não foi criado | Deletar `setor_armarios.sqlite` e reiniciar |
| Nenhum vestiário aparece | Dados não foram inseridos | Verificar se `InserirVestiario()` foi chamado |
| Nenhum armário aparece | `GetArmariosComStatus()` retorna null | Verificar se tabela armario tem dados |
| "Object reference not set" | Variável null sem check | Agora protegido por `DataTable?` |
| Armário não muda de cor | Cast inseguro falhou | Agora protegido por `is not` pattern |

---

## 🎓 APRENDIZADOS

### Problema vs Solução
1. **Inicialização:** Arquivos de BD precisam de setup automático
2. **Nullability:** C# 12 força explicitação de tipos nulláveis
3. **Segurança de Tipo:** `is` operator é mais seguro que cast
4. **Transações:** Sempre faça rollback em caso de erro
5. **Type-Safety:** `DataTable?` previne muitos bugs

### Boas Práticas Aplicadas
- ✅ Inicialização automática no `Main()`
- ✅ Tratamento de exceção com MessageBox
- ✅ Type-safe pattern matching
- ✅ Nullable reference types
- ✅ Transações com rollback
- ✅ Logging de erros

---

**Status:** 🟢 Pronto para testes em produção!
