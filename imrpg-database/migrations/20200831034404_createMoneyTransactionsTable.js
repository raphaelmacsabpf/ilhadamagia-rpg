exports.up = function(knex) {
    return knex.schema.createTable('imtb_money_transaction', function(t) {
        t.increments('Id').unsigned().primary();
        t.integer('SenderId').unsigned().notNull();
        t.integer('ReceiverId').unsigned().notNull();
        t.bigInteger('Ammount').notNull();
        t.string('Type').notNull();
        t.dateTime('CreatedAt').notNull();
        t.dateTime('FinishedAt').defaultTo(null);
        t.string('Status').notNull();
    });
};

exports.down = function(knex) {
    return knex.schema.dropTable('imtb_money_transaction');
};