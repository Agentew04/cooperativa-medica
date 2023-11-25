using System.Reflection.Metadata.Ecma335;
using MySql.Data.MySqlClient;
using CoopMedica.Database;
using CoopMedica.Models;

namespace CoopMedica.Services;

/// <summary>
/// Classe que gerencia a logica de criar uma conexão com o banco de dados.
/// </summary>
public class DatabaseService
{

    /// <summary>
    /// A unica instancia da classe DatabaseService.
    /// </summary>
    /// <returns></returns>
    public static DatabaseService Instance { get; } = new();

    private DatabaseService()
    {
        // Singleton
        string connectionString = @$"server=localhost;userid={MysqlData.Username};password={MysqlData.Password};database=cooperativa";

        connection = new MySqlConnection(connectionString);
        isOpen = false;
    }

    private bool isOpen;

    private MySqlConnection connection;

    /// <summary>
    /// Propriedade com a conexão com o banco de dados. 
    /// A abertura da conexao é <i>lazy</i>, ou seja, só é aberta quando a propriedade é acessada.
    /// </summary>
    public MySqlConnection Connection
    {
        get
        {
            if (!isOpen)
            {
                connection.Open();
                isOpen = true;
            }
            return connection;
        }
        private set => connection = value;
    }

    public async Task SetupDatabase()
    {
        MySqlCommand cmd;
        await DeleteAll();

        string plansTable = """
        CREATE TABLE IF NOT EXISTS `cooperativa`.`plans` (
        `plan_id` INT NOT NULL AUTO_INCREMENT,
        `nome` VARCHAR(45) NOT NULL,
        `desconto` FLOAT NOT NULL,
        `preco` FLOAT NOT NULL,
        PRIMARY KEY (`plan_id`));
        """;
        cmd = new(plansTable, Connection);
        await cmd.ExecuteNonQueryAsync();

        string specialitiesTable = """
        CREATE TABLE IF NOT EXISTS `cooperativa`.`specialities` (
        `speciality_id` INT NOT NULL AUTO_INCREMENT,
        `nome` VARCHAR(45) NOT NULL,
        PRIMARY KEY (`speciality_id`));
        """;
        cmd = new(specialitiesTable, Connection);
        await cmd.ExecuteNonQueryAsync();

        string affiliatedEntitiesTable = """
        CREATE TABLE IF NOT EXISTS `cooperativa`.`affiliated_entities` (
        `affiliated_entity_id` INT NOT NULL AUTO_INCREMENT,
        `nome` VARCHAR(45) NOT NULL,
        `cnpj` VARCHAR(45) NOT NULL,
        PRIMARY KEY (`affiliated_entity_id`));
        """;
        cmd = new(affiliatedEntitiesTable, Connection);
        await cmd.ExecuteNonQueryAsync();

        string banksTable = """
        CREATE TABLE IF NOT EXISTS `cooperativa`.`banks` (
        `bank_id` INT NOT NULL AUTO_INCREMENT,
        `nome` VARCHAR(45) NULL,
        PRIMARY KEY(`bank_id`)
        );
        """;
        cmd = new(banksTable, Connection);
        await cmd.ExecuteNonQueryAsync();

        string clientsTable = """
        CREATE TABLE IF NOT EXISTS `cooperativa`.`clients` (
        `client_id` INT NOT NULL AUTO_INCREMENT,
        `nome` VARCHAR(45) NOT NULL,
        `cpf` VARCHAR(45) NOT NULL,
        `data_nasc` DATE NOT NULL,
        `plan_id` INT NULL,
        PRIMARY KEY (`client_id`),
        FOREIGN KEY (`plan_id`) REFERENCES `cooperativa`.`plans` (`plan_id`));
        """;
        cmd = new(clientsTable, Connection);
        await cmd.ExecuteNonQueryAsync();

        string dependantsTable = """
        CREATE TABLE IF NOT EXISTS `cooperativa`.`dependants` (
        `dependant_id` INT NOT NULL AUTO_INCREMENT,
        `nome` VARCHAR(45) NULL,
        `client_id` INT NULL,
        PRIMARY KEY (`dependant_id`),
        FOREIGN KEY (`client_id`) REFERENCES `cooperativa`.`clients` (`client_id`));
        """;
        cmd = new(dependantsTable, Connection);
        await cmd.ExecuteNonQueryAsync();

        string servicesTable = """
        CREATE TABLE IF NOT EXISTS `cooperativa`.`services` (
        `service_id` INT NOT NULL AUTO_INCREMENT,
        `nome` VARCHAR(45) NOT NULL,
        `preco` FLOAT NOT NULL,
        `speciality_id` INT NOT NULL,
        `client_id` INT NOT NULL,
        `affiliated_entity_id` INT NOT NULL,
        PRIMARY KEY (`service_id`),
        FOREIGN KEY (`speciality_id`) REFERENCES `cooperativa`.`specialities` (`speciality_id`),
        FOREIGN KEY (`client_id`) REFERENCES `cooperativa`.`clients` (`client_id`),
        FOREIGN KEY (`affiliated_entity_id`) REFERENCES `cooperativa`.`affiliated_entities` (`affiliated_entity_id`));
        """;
        cmd = new(servicesTable, Connection);
        await cmd.ExecuteNonQueryAsync();

        string medicsTable = """
        CREATE TABLE IF NOT EXISTS `cooperativa`.`medics` (
        `medic_id` INT NOT NULL AUTO_INCREMENT,
        `nome` VARCHAR(45) NOT NULL,
        `speciality_id` INT NOT NULL,
        `affiliated_entity_id` INT NOT NULL,
        PRIMARY KEY (`medic_id`),
        FOREIGN KEY (`speciality_id`) REFERENCES `cooperativa`.`specialities` (`speciality_id`),
        FOREIGN KEY (`affiliated_entity_id`) REFERENCES `cooperativa`.`affiliated_entities` (`affiliated_entity_id`));
        """;
        cmd = new(medicsTable, Connection);
        await cmd.ExecuteNonQueryAsync();

        string clientPaymentsTable = """
        CREATE TABLE IF NOT EXISTS `cooperativa`.`client_payments` (
        `client_payment_id` INT NOT NULL AUTO_INCREMENT,
        `client_id` INT NOT NULL,
        `bank_id` INT NOT NULL,
        `valor` FLOAT NOT NULL,
        PRIMARY KEY (`client_payment_id`),
        FOREIGN KEY (`client_id`) REFERENCES `cooperativa`.`clients` (`client_id`),
        FOREIGN KEY (`bank_id`) REFERENCES `cooperativa`.`banks` (`bank_id`));
        """;
        cmd = new(clientPaymentsTable, Connection);
        await cmd.ExecuteNonQueryAsync();

        string entityPaymentsTable = """
        CREATE TABLE IF NOT EXISTS `cooperativa`.`entity_payments` (
        `entity_payment_id` INT NOT NULL AUTO_INCREMENT,
        `affiliated_entity_id` INT NOT NULL,
        `bank_id` INT NOT NULL,
        `valor` FLOAT NOT NULL,
        PRIMARY KEY (`entity_payment_id`),
        FOREIGN KEY (`affiliated_entity_id`) REFERENCES `cooperativa`.`affiliated_entities` (`affiliated_entity_id`),
        FOREIGN KEY (`bank_id`) REFERENCES `cooperativa`.`banks` (`bank_id`));
        """;
        cmd = new(entityPaymentsTable, Connection);
        await cmd.ExecuteNonQueryAsync();

        await AddValues();
    }

    private async Task DeleteAll()
    {
        string deleteAll = """
        DROP TABLE IF EXISTS cooperativa.services;
        DROP TABLE IF EXISTS cooperativa.dependants;
        DROP TABLE IF EXISTS cooperativa.client_payments;
        DROP TABLE IF EXISTS cooperativa.entity_payments;
        DROP TABLE IF EXISTS cooperativa.clients;
        DROP TABLE IF EXISTS cooperativa.plans;
        DROP TABLE IF EXISTS cooperativa.medics;
        DROP TABLE IF EXISTS cooperativa.specialities;
        DROP TABLE IF EXISTS cooperativa.affiliated_entities;
        DROP TABLE IF EXISTS cooperativa.banks;
        """;
        MySqlCommand cmd = new(deleteAll, Connection);
        await cmd.ExecuteNonQueryAsync();
    }

    private async Task AddValues()
    {
        PlanCollection planCollection = new();
        await planCollection.AddAsync(new Plan()
        {
            Name = "Unimed",
            Discount = 0.9f,
            Price = 1000
        });
        await planCollection.AddAsync(new Plan()
        {
            Name = "Saude Caixa",
            Discount = 0.8f,
            Price = 800
        });
        await planCollection.AddAsync(new Plan()
        {
            Name = "Ipe",
            Discount = 0.7f,
            Price = 600
        });

        ClientCollection clienteCollection = new();
        await clienteCollection.AddAsync(new Client()
        {
            Nome = "João",
            Cpf = "123.456.789-10",
            DataNascimento = DateOnly.FromDateTime(new DateTime(2000, 01, 01)),
            Plan = new Plan()
            {
                Id = 1,
            }
        });
        await clienteCollection.AddAsync(new Client()
        {
            Nome = "Pedro",
            Cpf = "312.231.415-12",
            DataNascimento = DateOnly.FromDateTime(new DateTime(2004, 05, 04)),
            Plan = new Plan()
            {
                Id = 2,
            }
        });
    }
}