
exports.up = function(knex) {
    return knex.schema.createTable('imtb_player_orgs', function(t) {
        t.increments('Id').unsigned().primary();
        t.string('Username').notNull();
        t.integer('OrgId').notNull();
        t.integer("Role").notNull();
    });
};

exports.down = function(knex) {
    return knex.schema.dropTable('imtb_player_orgs');
};
