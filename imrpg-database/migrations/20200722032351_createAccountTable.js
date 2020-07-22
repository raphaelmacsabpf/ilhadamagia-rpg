
exports.up = function(knex) {
    return knex.schema.createTable('imtb_account', function(t) {
        t.increments('Id').unsigned().primary();
        t.string('License').notNull();
        t.string('Username').notNull();
        t.string('Password').notNull();
        t.dateTime('CreatedAt').notNull();
        t.dateTime('UpdatedAt').nullable();
    });
};

exports.down = function(knex) {
    return knex.schema.dropTable('imtb_account');
};
