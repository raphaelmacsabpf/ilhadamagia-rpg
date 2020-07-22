module.exports = {

  development: {
    client: 'mysql2',
    connection: {
      host: '192.168.99.100',
      database: 'ilhadamagia',
      user:     'root',
      password: 'PQPMELANCIAAZEDA'
    },
    pool: {
      min: 2,
      max: 10
    },
    migrations: {
      tableName: 'knex_migrations'
    }
  },

  staging: {
    client: 'mysql2',
    connection: {
      host: '192.168.99.100',
      database: 'ilhadamagia',
      user:     'root',
      password: 'PQPMELANCIAAZEDA'
    },
    pool: {
      min: 2,
      max: 10
    },
    migrations: {
      tableName: 'knex_migrations'
    }
  },

  production: {
    client: 'mysql2',
    connection: {
      host: '192.168.99.100',
      database: 'ilhadamagia',
      user:     'root',
      password: 'PQPMELANCIAAZEDA'
    },
    pool: {
      min: 2,
      max: 10
    },
    migrations: {
      tableName: 'knex_migrations'
    }
  }
};
