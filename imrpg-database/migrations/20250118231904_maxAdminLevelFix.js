exports.up = function(knex) {
return knex.schema.alterTable('imtb_account', function(table) {
        table.integer('MaxAdminLevel').notNullable().defaultTo(0).alter();
    });
};

exports.down = function(knex) {
    return knex.schema.alterTable('imtb_account', function(table) {
        table.integer('MaxAdminLevel').nullable().defaultTo(null).alter();
    });
};