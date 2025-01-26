exports.up = function(knex) {
    return knex.schema.alterTable('imtb_account', function(table) {
        table.integer('MaxAdminLevel').after('AdminLevel');
    });
};

exports.down = function(knex) {
    return knex.schema.alterTable('imtb_account', function(table) {
        table.dropColumn('MaxAdminLevel');
    });
};