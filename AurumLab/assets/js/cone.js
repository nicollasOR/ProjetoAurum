let ano = parseInt(prompt("Digite um ano ai "));

if ((ano % 4 === 0 && ano % 100 !== 0) || (ano % 400 === 0)) 
  alert('${ano} eh um ano bissexto');
 else 
  alert('${ano} nao eh um ano bissexto');

