
exports.up = function(knex) {
    return knex.schema.alterTable('imtb_account', function(t) {
        t.specificType('IsLeader', 'TINYINT(1)').notNull().defaultTo(0);
    });
};

exports.down = function(knex) {
  return knex.schema.alterTable('imtb_account', function(t) {
    t.dropColumn('IsLeader');
  });
};
