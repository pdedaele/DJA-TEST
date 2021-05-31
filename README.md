# DJA-TEST

Pas voor het compileren de connectionstring aan in de App.config file van het project OrderConsumer.
Er zijn twee connectionstrings. 
Een voor het aanmaken van de DB, welke normaal niet aangepast moet worden. De database wordt aangemaakt in de default directory van SQL SERVER.
Ikzelf heb om mijn PC SQL SERVER V14 draaien.

De tweede string is voor het benaderen van de database. Deze zal je moeten aanpassen.

<connectionStrings>
    <add name="myDatabase"
         providerName ="System.Data.SqlClient"
         connectionString ="Integrated Security = SSPI; 
            Initial Catalog=myDatabase; 
            Data Source=localhost" />
    <add name="CreateDatabase"
         providerName ="System.Data.SqlClient"
         connectionString ="Server=localhost;Integrated security=SSPI;database=master" />
  </connectionStrings>
  
  Elke project draait volledig onafhankelijk van elkaar.
  OrderGenerator maakt de orders aan in C:\Orders 
