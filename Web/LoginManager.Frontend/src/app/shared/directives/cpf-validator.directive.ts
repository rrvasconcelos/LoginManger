import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const CpfValidator: ValidatorFn = (
  control: AbstractControl
): ValidationErrors | null => {
  const value = control.value || '';

  // Se estiver vazio, deixa a validação de required fazer seu trabalho
  if (!value) {
    return null;
  }

  // Verificar se o valor contém apenas números e pontuação válida
  if (!/^[\d.-]*$/.test(value)) {
    return { cpfInvalid: true };
  }

  // Remover caracteres não numéricos para validação
  const cpf = value.replace(/[^\d]+/g, '');

  // Verificar se tem 11 dígitos após a limpeza
  if (cpf.length !== 11) {
    return { cpfInvalid: true };
  }

  // Elimina CPFs inválidos conhecidos
  if (
    cpf === '00000000000' ||
    cpf === '11111111111' ||
    cpf === '22222222222' ||
    cpf === '33333333333' ||
    cpf === '44444444444' ||
    cpf === '55555555555' ||
    cpf === '66666666666' ||
    cpf === '77777777777' ||
    cpf === '88888888888' ||
    cpf === '99999999999'
  ) {
    return { cpfInvalid: true };
  }

  // Valida 1o dígito
  let add = 0;
  for (let i = 0; i < 9; i++) {
    add += parseInt(cpf.charAt(i)) * (10 - i);
  }
  let rev = 11 - (add % 11);
  if (rev === 10 || rev === 11) {
    rev = 0;
  }
  if (rev !== parseInt(cpf.charAt(9))) {
    return { cpfInvalid: true };
  }

  // Valida 2o dígito
  add = 0;
  for (let i = 0; i < 10; i++) {
    add += parseInt(cpf.charAt(i)) * (11 - i);
  }
  rev = 11 - (add % 11);
  if (rev === 10 || rev === 11) {
    rev = 0;
  }
  if (rev !== parseInt(cpf.charAt(10))) {
    return { cpfInvalid: true };
  }

  return null;
};
